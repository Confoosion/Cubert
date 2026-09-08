using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public interface ITickable
{
    void Tick(float deltaTime);
}

public class TimeManager : MonoBehaviour
{
    public static TimeManager Singleton;
    private readonly List<ITickable> tickables = new List<ITickable>();
    private readonly List<ITickable> pendingAdds = new List<ITickable>();
    private readonly List<ITickable> pendingRemoves = new List<ITickable>();

    private float tickInterval = 5f;
    private float timer = 0f;

    // [SerializeField] private TextMeshProUGUI timeText;
    private int START_HOURS = 9;
    private int START_MINUTES = 0;
    private int hours;
    private float minutes;
    private bool isAM = true;
    public bool IsAM => isAM;

    private float timeInterval;

    [SerializeField] private bool timeFrozen = false;

    [SerializeField] private bool isMorning = true;
    public bool IsMorning => isMorning;

    void Awake() { if(Singleton == null) Singleton = this; }

    public static void Register(ITickable tickable) => Singleton.pendingAdds.Add(tickable);
    public static void Unregister(ITickable tickable) => Singleton.pendingRemoves.Add(tickable);

    void Update()
    {
        if(timeFrozen) return;

        if(pendingAdds.Count > 0)
        {
            tickables.AddRange(pendingAdds);
            pendingAdds.Clear();
        }
        if(pendingRemoves.Count > 0)
        {
            foreach(var t in pendingRemoves) tickables.Remove(t);
            pendingRemoves.Clear();
        }

        if(timer >= tickInterval)
        {
            timer = 0f;
            float dt = Time.deltaTime;
            for(int i = 0; i < tickables.Count; i++)
            {
                tickables[i].Tick(dt);
            }
            Debug.Log("Tick");
        }

        timer += Time.deltaTime;
    }

    public void FreezeTime(bool freeze)
    {
        timeFrozen = freeze;
    }

    public void ResetTime()
    {
        isAM = true;
        string mins = "";
        mins += (START_MINUTES < 10) ? "0" + START_MINUTES.ToString() : START_MINUTES.ToString();

        string ampm = isAM ? " AM" : " PM";

        // timeText.SetText(START_HOURS.ToString() + ":" + mins + ampm);

        hours = START_HOURS;
        minutes = (float)START_MINUTES;
    }

    public void UpdateTime()
    {
        string mins = "";
        mins += (int)minutes < 10 ? "0" + ((int)minutes).ToString() : ((int)minutes).ToString();

        string ampm = isAM ? " AM" : " PM";

        // timeText.SetText(hours.ToString() + ":" + mins + ampm);
    }

    public void MoveTimeForward()
    {
        minutes += timeInterval;
        
        int hoursToAdd = (int)minutes / 60;
        minutes -= 60f * hoursToAdd;
        hours += hoursToAdd;

        if(hours > 12)
        {
            hours = 1;
            isAM = false;
        }
        else if(hours > 11)
        {
            isAM = false;
        }


        UpdateTime();
    }

    public void CalculateTimeInterval(int totalNeeds)
    {
        timeInterval = 480f / (float)totalNeeds;
    }

    public void SetNight()
    {
        isMorning = false;
    }
}
