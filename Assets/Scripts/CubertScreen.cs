using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CubertScreen : MonoBehaviour
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
    private BoxCollider2D nestCollider;

    private Transform cubertTransform;
    public Transform CubertTransform => cubertTransform;
    private Cubert cubert;
    public Cubert _Cubert => cubert;

    // Time Variables
    private float sleepTime = 7f;
    private float pottyTime = 4f;
    private float feedCooldown = 0.5f;
    private float feedTimer = 0f;
    private float timeInSpot = 0f;
    private float timeNeededBeforeHabit = 0f;
    private float timeAway = 0f;
    private float needCooldown = 1.5f;
    private float needTimer = 0f;

    [Header("Sounds")]
    [SerializeField] private AudioClip fartSound;
    [SerializeField] private AudioSource lightSwitchSource;
    [SerializeField] private AudioClip[] lubertSounds;

    [Header("Misc")]
    [SerializeField] private SpriteRenderer bedRenderer;

    [SerializeField] private NeedsSO currentNeed = null;
    [SerializeField] private NeedsSO[] basicNeeds;
    [SerializeField] private NeedsSO[] specialNeeds;
    private LinkedList<NeedsSO> needList = new LinkedList<NeedsSO>();
    public NeedsSO CurrentNeed => currentNeed;
    
    private int minNeeds = 1;
    private int maxNeeds = 3;
    public int MinNeeds => minNeeds;
    public int MaxNeeds => maxNeeds;

    private Habit[] habits;
    private bool habitPerformed;

    private int foodAte = 0;

    private bool isEnhanced = false;

    void Start()
    {
        lightsOn = true;
        roomText.SetText(gameObject.name + "'s Room");
        statusText.SetText("");
        needTimer = needCooldown;
        nestCollider = nest.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        TickTime();

        if(isEnhanced && ScreenManager.Singleton.CurrentScreen == transform)
        {
            isEnhanced = false;
            EnhancedScare();
        }
    }

    private void TickTime()
    {
        if(TimeManager.Singleton.IsTimeFrozen) return;

        if(feedTimer > 0f)
        {
            feedTimer -= Time.deltaTime;
        }

        if(currentNeed == null && cubert != null && needList.Count > 0)
        {
            if(needTimer >= 0f)
            {
                needTimer -= Time.deltaTime;
            }
            else
            {
                currentNeed = needList.First.Value;
                needTimer = needCooldown;
                UpdateNeedStatus();
            }
        }
        
        if(currentNeed != null)
        {
            if(currentNeed.needName == "Tired" && currentSpot == bed)
            {
                timeInSpot += Time.deltaTime;
                if(timeInSpot >= sleepTime)
                {
                    cubert.DisplaySleepParticles(false);
                    SatisfyNeed();
                }
            }
            else if(currentNeed.needName == "Potty" && currentSpot == litterBox)
            {
                timeInSpot += Time.deltaTime;
                if(timeInSpot >= pottyTime)
                {
                    if(transform == ScreenManager.Singleton.CurrentScreen)
                    {
                        SoundManager.Singleton?.PlaySFX(fartSound);
                    }
                    SatisfyNeed();
                }
            }
            else if(currentNeed.needName == "Scared")
            {
                if(cubert.gameObject.name is "Oubert" or "Tubert")
                {
                    if(!transform.Find("Hubert"))
                    {
                        SatisfyNeed();
                    }
                }
            }
        }

        if(ScreenManager.Singleton.CurrentScreen != transform)
        {
            if(habits == null || TimeManager.Singleton.IsNight) return;
            if(HoldCubert.Singleton.HoldingCubert && HoldCubert.Singleton.HeldCubert == cubert) return;
            if(cubert.gameObject.name == "Cubert" && transform.Find("Mubert")) return;
            if(habitPerformed) return;

            timeAway += Time.deltaTime;

            if(timeAway >= timeNeededBeforeHabit && !habitPerformed)
            {
                timeAway = 0f;
                habitPerformed = true;
                Habit habitToDo;
                if(habits.Length > 1)
                {
                    habitToDo = habits[UnityEngine.Random.Range(0, habits.Length)];
                }
                else
                {
                    habitToDo = habits[0];
                }

                PerformHabit(habitToDo);
            }
        }
    }

    public void SetBedSprite(Sprite bed)
    {
        bedRenderer.sprite = bed;
    }

    public void SetCubert(GameObject _cubert)
    {
        _cubert.GetComponent<BoxCollider2D>().enabled = true;
        _cubert.transform.SetParent(transform);
        _cubert.transform.localPosition = nest.localPosition;
        _cubert.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);

        currentSpot = nest;

        DisplayFeedButton(true);

        cubertTransform = _cubert.transform;
        cubert = _cubert.GetComponent<Cubert>();
        cubert.SetHome(this);

        SetCubertHabits();
    }

    private void SetCubertHabits()
    {
        if(Enum.TryParse(SceneManager.GetActiveScene().name, out Day currentDay))
        {
            CubertNeedsSO cubertNeeds = cubert.CubertNeeds;
            if(cubertNeeds.cubertHabits == null) return;

            foreach(var entry in cubertNeeds.cubertHabits)
            {
                if(entry.day == currentDay)
                {
                    habits = entry.habits;
                    timeNeededBeforeHabit = entry.timeNeeded;
                    break;
                }
            }
        }
    }

    public bool PlaceCubert(GameObject _cubert)
    {
        if(_cubert.name == gameObject.name)
        {
            _cubert.GetComponent<BoxCollider2D>().enabled = true;
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = nest.localPosition;
            _cubert.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);

            currentSpot = nest;

            DisplayFeedButton(true);

            if(cubert == null)
            {
                cubertTransform = _cubert.transform;
                cubert = _cubert.GetComponent<Cubert>();
                cubert.SetHome(this);

                DaycareScreen.Singleton.GetNextNPC();

                SetCubertHabits();
            }

            GameObject dayFind = GameObject.Find("TuesdayStuff");
            if(cubert.gameObject.name == "Mubert" && dayFind != null && TimeManager.Singleton.IsNight)
            {
                dayFind.GetComponent<TuesdayStuff>().DisplayHiddenMuberts(true);
            }

            if(currentNeed?.name == "Missing")
            {
                SatisfyNeed();
            }

            nestCollider.enabled = false;

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
            _cubert.transform.localPosition = litterBox.localPosition + new Vector3(-0.5f, 1.45f, 0f);
            _cubert.transform.localScale = new Vector3(2f, 2f, 2f);
            _cubert.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
            
            currentSpot = litterBox;

            return true;
        }
        
        return false;
    }

    public void ForceCubertInLitterBox()
    {
        timeInSpot = 0f;
        cubert.transform.SetParent(transform);
        cubert.transform.localPosition = litterBox.localPosition + new Vector3(-0.5f, 1.45f, 0f);
        cubert.transform.localScale = new Vector3(2f, 2f, 2f);
        cubert.GetComponent<SpriteRenderer>().sortingLayerName = "Background";

        currentSpot = litterBox;
        DisplayFeedButton(false);
    }

    public bool PlaceCubertInBed(GameObject _cubert)
    {
        Debug.Log("Bed");
        if(_cubert.name == gameObject.name && currentNeed?.needName == "Tired")
        {
            timeInSpot = 0f;
            _cubert.GetComponent<BoxCollider2D>().enabled = true;
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = bed.localPosition + new Vector3(0f, 0.8f, 0f);
            _cubert.transform.localScale = new Vector3(2f, 2f, 2f);
            _cubert.GetComponent<SpriteRenderer>().sortingLayerName = "Background";

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
        if(feedTimer > 0f) return;
        if(FoodManager.Singleton.FoodAmount < 1) return;

        feedTimer = feedCooldown;
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
            SoundManager.Singleton?.PlayEatSFX();
        }

        if(currentNeed?.needName == "Hungry")
        {
            foodAte += 1;
        }
        else yield return null;
        
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

        lightSwitchSource.Play();
    }

    public void TurnLightsOff()
    {
        if(!lightsOn) return;
        ToggleLightSwitch();
    }

    // NEEDS
    public int QueueNeeds()
    {
        CubertNeedsSO cubertNeeds = cubert.CubertNeeds;
        int needs = 0;

        if(Enum.TryParse(SceneManager.GetActiveScene().name, out Day currentDay))
        {
            if(cubertNeeds == null)
            {
                Debug.LogWarning(cubert + " has no Needs variable assigned!");
                return 0;
            }

            foreach(var entry in cubertNeeds.cubertNeeds)
            {
                if(entry.day == currentDay)
                {
                    foreach(NeedsSO need in entry.needs)
                    {
                        needList.AddLast(need);
                        needs++;
                    }
                }
            }
        }

        return needs;
    }

    private void ForceAddNeed(NeedsSO need)
    {
        needList.AddFirst(need);
        DaycareScreen.Singleton.AddNeed();
        currentNeed = needList.First.Value;
        UpdateNeedStatus();
    }

    public void ForceAddScared()
    {
        ForceAddNeed(specialNeeds[1]);
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
            case "Dirty":
                {
                    if(cubert.gameObject.name == "Lubert")
                    {
                        cubert.GetComponent<LubertSounds>().PlayStinkySound();
                    }
                    break;
                }
            case "Potty":
                {
                    if(cubert.gameObject.name == "Lubert")
                    {
                        cubert.GetComponent<LubertSounds>().PlayChopChopSound();
                    }
                    break;
                }
            case "Scared":
                {
                    DisplayFeedButton(false);
                    break;
                }
        }

        ScreenManager.Singleton.SetNeedScreen(transform);
        statusText.SetText(cubert.gameObject.name + " " + currentNeed.description);
    }

    public void SatisfyNeed()
    {
        if(cubert.gameObject.name == "Lubert" && currentNeed.name == "Hungry")
        {
            cubert.GetComponent<LubertSounds>().PlayBlandSound();
        }

        if(currentNeed.needName is "Dirty" or "Tired")
        {
            PlaceCubert(cubert.gameObject);
        }

        needList.RemoveFirst();
        timeInSpot = 0f;
        currentNeed = null;
        UpdateNeedStatus();
        DaycareScreen.Singleton.NeedSatisfied();
        habitPerformed = false;

        if(currentSpot == nest)
        {
            DisplayFeedButton(true);
        }
    }

    public void PerformHabit(Habit habit)
    {
        switch(habit)
        {
            case Habit.UpClose:
                {
                    EnhanceCubert();
                    break;        
                }
            case Habit.LightsOff:
                {
                    TurnLightsOff();
                    break;
                }
            case Habit.HamsterSleep:
                {
                    ForceCubertInLitterBox();
                    cubert.DisplaySleepParticles(true);
                    ForceAddNeed(basicNeeds[1]);
                    break;
                }
            case Habit.MubertLeave:
                {
                    TurnLightsOff();
                    if(cubert.GetComponent<MubertStuff>().HideInCubertsRoom(this))
                        ForceAddNeed(specialNeeds[0]);
                    break;
                }
            case Habit.HubertLeave:
                {
                    if(cubert.GetComponent<HubertStuff>().AttemptLeave(this))
                    {
                        ForceAddNeed(specialNeeds[0]);
                    }
                    break;
                }
            case Habit.JubertMove:
                {
                    
                    break;
                }
            
        }
    }

    private void EnhanceCubert()
    {
        currentSpot = null;
        timeInSpot = 0f;
        cubertTransform.localPosition = nest.localPosition;
        isEnhanced = true;
        cubertTransform.localScale = new Vector3(17f, 17f, 17f);
    }

    public void EnhancedScare()
    {
        Debug.Log("Enhanced Jumpscare SFX!");
    }

    public void PlayLubertSound()
    {
        if(cubert != null && HoldCubert.Singleton.HeldCubert != cubert)
            SoundManager.Singleton?.PlaySFX(lubertSounds[UnityEngine.Random.Range(0, lubertSounds.Length)]);
    }

    public void EnableNestCollider()
    {
        nestCollider.enabled = true;
    }
}
