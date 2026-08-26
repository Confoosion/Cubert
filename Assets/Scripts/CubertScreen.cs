using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CubertScreen : MonoBehaviour, ITickable
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private GameObject foodParticle;
    [SerializeField] private Transform foodTransform;

    [SerializeField] private GameObject foodButton;
    [SerializeField] private TextMeshProUGUI roomText;
    [SerializeField] private TextMeshProUGUI statusText;

    [Header("Cubert Room Spots")]
    [SerializeField] private Transform nest;
    [SerializeField] private Transform litterBox;
    [SerializeField] private Transform bed;
    [SerializeField] private Transform bathtub;
    [SerializeField] private Transform playArea;

    private Transform cubertTransform;
    private Cubert cubert;

    private float feedCooldown = 1f;
    private float timer = 0f;

    [System.Serializable]
    public class Need
    {
        public NeedsSO need;
        public float needChance;
    }

    [SerializeField] private Need[] needs;
    private Need currentNeed;
    public Need CurrentNeed => currentNeed;

    private int foodAte = 0;

    private void OnEnable() => TimeManager.Register(this);
    private void OnDisable() => TimeManager.Unregister(this);

    void Start()
    {
        roomText.SetText(gameObject.name + "'s Room");
        statusText.SetText("");
    }

    void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Tick(float deltaTime)
    {
        if(currentNeed == null && cubert != null)
        {
            currentNeed = GetRandomNeed();
            UpdateNeedStatus();
        }
    }

    public bool PlaceCubert(GameObject _cubert)
    {
        if(_cubert.name == gameObject.name)
        {
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = nest.localPosition;
            _cubert.transform.localScale = new Vector3(4f, 4f, 4f);

            foodButton.SetActive(true);

            if(cubert == null)
            {
                cubertTransform = _cubert.transform;
                cubert = _cubert.GetComponent<Cubert>();
                cubert.SetHome(this);
                needs = cubert.Needs;
            }

            return true;
        }

        return false;
    }

    public bool PlaceCubertInLitterBox(GameObject _cubert)
    {
        Debug.Log("Litter box");
        if(_cubert.name == gameObject.name && currentNeed?.need.needName == "Potty")
        {
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = litterBox.localPosition;
            _cubert.transform.localScale = new Vector3(3f, 3f, 3f);
            
            return true;
        }
        
        return false;
    }

    public bool PlaceCubertInBed(GameObject _cubert)
    {
        Debug.Log("Bed");
        if(_cubert.name == gameObject.name && currentNeed?.need.needName == "Tired")
        {
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = bed.localPosition;
            _cubert.transform.localScale = new Vector3(3f, 3f, 3f);
            
            return true;
        }
        
        return false;
    }

    public bool PlaceCubertInBathtub(GameObject _cubert)
    {
        Debug.Log("Bathtub");
        if(_cubert.name == gameObject.name && currentNeed?.need.needName == "Dirty")
        {
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = bathtub.localPosition;
            _cubert.transform.localScale = new Vector3(3f, 3f, 3f);
            
            return true;
        }
        
        return false;
    }

    public bool PlaceCubertInPlayArea(GameObject _cubert)
    {
        Debug.Log("Play area");
        if(_cubert.name == gameObject.name && currentNeed?.need.needName == "Bored")
        {
            _cubert.transform.SetParent(transform);
            _cubert.transform.localPosition = playArea.localPosition;
            _cubert.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            return true;
        }

        return false;
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

        foodAte += 1;
        if(foodAte >= cubert.FeedAmount)
        {
            SatisfyNeed();
        }
    }

    // NEEDS
    private Need GetRandomNeed()
    {
        float totalProbability = 0f;
        for(int i = 0; i < needs.Length; i++)
        {
            totalProbability += needs[i].needChance;
        }
        
        float roll = Random.value * totalProbability;
        float cumulative = 0f;
        for(int i = 0; i < needs.Length; i++)
        {
            cumulative += needs[i].needChance;
            if(roll <= cumulative)
            {
                return needs[i];
            }
        }

        return(null);
    }

    private void UpdateNeedStatus()
    {
        if(currentNeed == null)
        {
            statusText.text = "";
            return;    
        }

        switch(currentNeed.need.needName)
        {
            case "Hungry":
                {
                    foodAte = 0;
                    break;
                }
        }

        statusText.SetText(cubert.gameObject.name + " " + currentNeed.need.description);
    }

    public void SatisfyNeed()
    {
        currentNeed = null;
        UpdateNeedStatus();
    }
}
