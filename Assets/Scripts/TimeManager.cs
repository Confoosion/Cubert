using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Singleton;

    private bool isAM = true;
    public bool IsAM => isAM;

    [SerializeField] private bool timeFrozen = false;
    public bool IsTimeFrozen => timeFrozen;
    [SerializeField] private bool isMorning = true;
    public bool IsMorning => isMorning;
    [SerializeField] private bool isNight = false;
    public bool IsNight => isNight;

    private float timer = 0f;
    private bool annaTimeStart = false;
    private float timeTillAnnaGetsAngry = 15f;

    void Awake() { if(Singleton == null) Singleton = this; }

    void Update()
    {
        if(annaTimeStart && timer < timeTillAnnaGetsAngry)
        {
            timer += Time.deltaTime;
            if(timer >= timeTillAnnaGetsAngry)
            {
                annaTimeStart = false;
                GlobalEvents.Singleton.GotAnnaAngry();
            }
        }
        
    }

    public void FreezeTime(bool freeze)
    {
        timeFrozen = freeze;
    }

    public void SetNight()
    {
        isMorning = false;
        isNight = true;
    }

    public string GetDay()
    {
        return(SceneManager.GetActiveScene().name);
    }

    public void StartAnnaTimer()
    {
        annaTimeStart = true;
    }
}
