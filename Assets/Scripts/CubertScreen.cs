using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CubertScreen : MonoBehaviour, ITickable
{
    [Header("Food")]
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private GameObject foodParticle;
    [SerializeField] private Transform foodTransform;
    [SerializeField] private GameObject foodButton;

    [Header("Room Identity")]
    [SerializeField] private TextMeshProUGUI roomText;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Lights")]
    [SerializeField] private Image darkness;
    private bool lightsOn = true;

    [Header("Cubert Room Spots")]
    [SerializeField] private Transform nest;
    [SerializeField] private Transform litterBox;
    [SerializeField] private Transform bed;
    private Transform currentSpot;

    private Transform cubertTransform;
    private Cubert cubert;
    public Cubert _Cubert => cubert;

    private float sleepTime = 7f;
    private float pottyTime = 4f;
    private float feedCooldown = 1f;
    private float timer = 0f;
    private float timeInSpot = 0f;

    [Header("Sounds")]
    [SerializeField] private AudioClip[] eatingSounds;
    [SerializeField] private AudioClip fartSound;

    [SerializeField] private NeedsSO currentNeed;
    [SerializeField] private NeedsSO[] needs;
    private Queue<NeedsSO> needQueue = new Queue<NeedsSO>();
    public NeedsSO CurrentNeed => currentNeed;
    
    private int minNeeds = 1;
    private int maxNeeds = 3;
    public int MinNeeds => minNeeds;
    public int MaxNeeds => maxNeeds;

    private int foodAte = 0;

    private void OnEnable() => TimeManager.Register(this);
    private void OnDisable() => TimeManager.Unregister(this);

    void Start()
    {
        lightsOn = true;
        roomText.SetText(gameObject.name + "'s Room");
        statusText.SetText("");
    }

    void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }

        if(currentNeed != null)
        {
            if(currentNeed.needName == "Tired" || currentNeed.needName == "Potty")
            {
                timeInSpot += Time.deltaTime;

                if(timeInSpot >= sleepTime && currentSpot == bed)
                {
                    cubert.DisplaySleepParticles(false);
                    SatisfyNeed();
                }
                else if(timeInSpot >= pottyTime && currentSpot == litterBox)
                {
                    if(transform == ScreenManager.Singleton.CurrentScreen)
                    {
                        SoundManager.Singleton?.PlaySFX(fartSound);
                    }

                    SatisfyNeed();
                }
            }
        }
    }

    public void Tick(float deltaTime)
    {
        if(currentNeed == null && cubert != null && needQueue.Count > 0)
        {
            currentNeed = needQueue.Dequeue();
            Debug.Log(currentNeed);
            UpdateNeedStatus();
        }
    }

    public bool PlaceCubert(GameObject _cubert)
    {
        if(_cubert.name == gameObject.name)
        {
            _cubert.GetComponent<BoxCollider2D>().enabled = true;
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = nest.localPosition;
            _cubert.transform.localScale = new Vector3(4f, 4f, 4f);

            currentSpot = nest;

            DisplayFeedButton(true);

            if(cubert == null)
            {
                cubertTransform = _cubert.transform;
                cubert = _cubert.GetComponent<Cubert>();
                cubert.SetHome(this);

                DaycareScreen.Singleton.GetNextNPC();
            }

            return true;
        }

        return false;
    }

    public bool PlaceCubertInLitterBox(GameObject _cubert)
    {
        Debug.Log("Litter box");
        if(_cubert.name == gameObject.name && currentNeed?.needName == "Potty")
        {
            timeInSpot = 0f;
            _cubert.GetComponent<BoxCollider2D>().enabled = true;
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = litterBox.localPosition;
            _cubert.transform.localScale = new Vector3(3f, 3f, 3f);
            
            currentSpot = litterBox;

            return true;
        }
        
        return false;
    }

    public bool PlaceCubertInBed(GameObject _cubert)
    {
        Debug.Log("Bed");
        if(_cubert.name == gameObject.name && currentNeed?.needName == "Tired")
        {
            timeInSpot = 0f;
            _cubert.GetComponent<BoxCollider2D>().enabled = true;
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = bed.localPosition;
            _cubert.transform.localScale = new Vector3(3f, 3f, 3f);
            
            currentSpot = bed;

            _cubert.GetComponent<Cubert>()?.DisplaySleepParticles(true);

            return true;
        }
        
        return false;
    }

    public void DisplayFeedButton(bool show)
    {
        foodButton.SetActive(show);
    }

    public void FeedCubert()
    {
        if(timer > 0f) return;
        if(FoodManager.Singleton.FoodAmount < 1) return;

        timer = feedCooldown;
        FoodManager.Singleton.UseFood(1);

        StartCoroutine(Feeding());
    }

    IEnumerator Feeding()
    {
        GameObject food = Instantiate(foodPrefab, foodTransform.position, Quaternion.identity);

        food.GetComponent<Rigidbody2D>().simulated = false;
        food.GetComponent<CircleCollider2D>().enabled = false;

        Transform target = cubertTransform;
        
        Vector3 startPos = food.transform.position;
        Vector3 startScale = food.transform.localScale;

        float duration = 0.35f;
        float _time = 0f;
        
        while(_time < duration)
        {
            _time += Time.deltaTime;
            float t = Mathf.Clamp01(_time / duration);
            float eased = 1f - Mathf.Pow(1f - t, 2f);

            food.transform.position = Vector3.Lerp(startPos, target.position, eased);
            food.transform.localScale = Vector3.Lerp(startScale, startScale * 0.3f, eased);
            
            yield return null;
        }

        food.transform.position = target.position;
        food.transform.localScale = startScale * 0.3f;

        Destroy(food.gameObject);
        Instantiate(foodParticle, new Vector3(cubertTransform.position.x, cubertTransform.position.y, -2f), Quaternion.identity);

        if(transform == ScreenManager.Singleton.CurrentScreen)
        {
            SoundManager.Singleton?.PlaySFX(eatingSounds[UnityEngine.Random.Range(0, eatingSounds.Length - 1)]);
        }

        if(currentNeed.needName == "Hungry")
        {
            foodAte += 1;
        }
        
        if(foodAte >= cubert.FeedAmount)
        {
            SatisfyNeed();
            foodAte = 0;
        }
    }

    public void ToggleLightSwitch()
    {
        lightsOn = !lightsOn;
        darkness.enabled = !lightsOn;

        if(cubert != null)
        {
            if(!lightsOn)
            {
                cubert.HideCubert();
            }
            else
            {
                cubert.ShowCubert();
            }
        }
    }

    // NEEDS
    public int QueueNeeds()
    {
        CubertNeedsSO cubertNeeds = cubert.CubertNeeds;
        int needs = 0;

        if(Enum.TryParse(SceneManager.GetActiveScene().name, out Day currentDay))
        {
            foreach(var entry in cubertNeeds.cubertNeeds)
            {
                if(entry.day == currentDay)
                {
                    foreach(NeedsSO need in entry.needs)
                    {
                        needQueue.Enqueue(need);
                        needs++;
                    }
                }
            }
        }

        return needs;
    }

    private void UpdateNeedStatus()
    {
        if(currentNeed == null)
        {
            statusText.text = "";
            ScreenManager.Singleton.RemoveNeedScreen(transform);
            return;    
        }

        switch(currentNeed.needName)
        {
            case "Hungry":
                {
                    foodAte = 0;
                    break;
                }
        }

        ScreenManager.Singleton.SetNeedScreen(transform);
        statusText.SetText(cubert.gameObject.name + " " + currentNeed.description);
    }

    public void SatisfyNeed()
    {
        timeInSpot = 0f;
        currentNeed = null;
        UpdateNeedStatus();
        DaycareScreen.Singleton.NeedSatisfied();
    }
}
