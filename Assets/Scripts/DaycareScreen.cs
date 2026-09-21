using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Purpose { DropOff, PickUp }

public class DaycareScreen : MonoBehaviour
{
    public static DaycareScreen Singleton;
    void Awake() { if(Singleton == null) Singleton = this; }

    [System.Serializable]
    public class NPCPerson
    {
        public NPCSO npcSO;
        public Purpose purpose;

        public NPCPerson(NPCSO _npcSO, Purpose _purpose)
        {
            npcSO = _npcSO;
            purpose = _purpose;
        }
    }

    [SerializeField] private List<NPCPerson> npcsInMorning = new List<NPCPerson>();
    [SerializeField] private List<NPCPerson> npcsInEvening = new List<NPCPerson>();

    private Queue<NPCPerson> npcQueue = new Queue<NPCPerson>();
    private NPCPerson currentNPC;
    public NPCPerson CurrentNPC => currentNPC;

    [SerializeField] private NPC npc;
    public NPC Npc => npc;

    [SerializeField] private Transform cubertHolder;

    [Header("Sounds")]
    [SerializeField] private AudioClip NPCEnterSound;

    private int todaysNeeds = 0;
    private int currentNeedsCompleted = 0;

    [Header("Misc")]
    [SerializeField] private BoxCollider2D cubertLocationCollider;
    [SerializeField] private BoxCollider2D customerCollider;

    void Start()
    {
        TimeManager.Singleton.FreezeTime(true);
        
        DisableCubertDropOff();

        for(int i = 0; i < npcsInMorning.Count; i++)
        {
            npcQueue.Enqueue(npcsInMorning[i]);
        }

        if(TimeManager.Singleton.GetDay() == "Friday" && GlobalEvents.Singleton.AnnaAngry)
        {
            npcQueue.Enqueue(new NPCPerson(GameObject.Find("FridayStuff").GetComponent<FridayStuff>().ChristinaNPC, Purpose.PickUp));
        }

        GetNextNPC();
    }

    public void DayOver()
    {
        Debug.Log("Day is fully completed!");
    }

    public void SpawnCubert()
    {
        GameObject cubert = Instantiate(npc.Data.cubert, cubertHolder.position, Quaternion.identity);
        cubert.name = npc.Data.cubert.name;
        ScreenManager.Singleton.AddCubert(npc.Data.cubert);
    }

    public void GetNextNPC()
    {
        if(npcQueue.Count > 0)
        {
            currentNPC = npcQueue.Dequeue();

            if(currentNPC.npcSO.npcName == "Anna" && TimeManager.Singleton.GetDay() == "Thursday" && GlobalEvents.Singleton.AnnaAngry)
            {
                currentNPC = new NPCPerson(GameObject.Find("ThursdayStuff").GetComponent<ThursdayStuff>().ChristinaNPC, Purpose.DropOff);
            }
            else if(currentNPC.npcSO.npcName == "Calvin" && GlobalEvents.Singleton.HubertDead)
            {
                currentNPC = npcQueue.Dequeue();
            }
            else if(currentNPC.npcSO.cubert.name == "Tubert" && GlobalEvents.Singleton.TubertDead)
            {
                currentNPC = npcQueue.Dequeue();
            }
            else if(currentNPC.npcSO.npcName == "Rose" && TimeManager.Singleton.GetDay() == "Friday" && TimeManager.Singleton.IsNight && GlobalEvents.Singleton.FubertMakeupRuined)
            {
                currentNPC = null;
                DayOver();
                return;
            }
            else if(currentNPC.npcSO.npcName == "Timothy" && TimeManager.Singleton.GetDay() == "Friday" && TimeManager.Singleton.IsNight && !GlobalEvents.Singleton.TubertDead && !GlobalEvents.Singleton.OubertDead)
            {
                GameObject.Find("FridayStuff").GetComponent<FridayStuff>().EnterTims();
                return;
            }

            npc.SetData(currentNPC.npcSO, currentNPC.purpose);
            NPCEnter();
        }
        else if(!TimeManager.Singleton.IsNight)
        {
            StartDay();
        }
        else
        {
            DayOver();
        }
    }

    public void NPCEnter()
    {
        npc.SetInteracted(false);
        npc.NPCEnter();
        SoundManager.Singleton?.PlaySFX(NPCEnterSound);
        ScreenManager.Singleton.SetNPCScreen(true);

        if(npc.Data.npcName == "Timothy" && TimeManager.Singleton.GetDay() == "Wednesday" && TimeManager.Singleton.IsNight)
        {
            GameObject.Find("WednesdayStuff").GetComponent<WednesdayStuff>().HubertLeave();
        }
    }

    public void NPCLeave()
    {
        npc.NPCLeave();
        ScreenManager.Singleton.SetNPCScreen(false);

        if(TimeManager.Singleton.GetDay() == "Friday" && currentNPC.npcSO.npcName == "Rose" && !TimeManager.Singleton.IsNight)
        {
            return;
        }

        if(TimeManager.Singleton.IsNight && currentNPC.purpose is Purpose.PickUp)
        {
            StartCoroutine(SpawnNightNPC());
        }
        else if(currentNPC.purpose is Purpose.PickUp)
        {
            StartCoroutine(SpawnMorningNPC());
        }

        currentNPC = null;
    }

    IEnumerator SpawnMorningNPC()
    {
        yield return new WaitForSeconds(2.5f);

        if(npcQueue.Count > 0)
        {
            GetNextNPC();
        }
        else
        {
            StartDay();
        }
    }

    IEnumerator SpawnNightNPC()
    {
        yield return new WaitForSeconds(2.5f);
        
        if(npcQueue.Count > 0)
        {
            GetNextNPC();
        }
        else
        {
            Debug.Log("Day is fully completed!");
        }
    }

    public void NeedSatisfied()
    {
        currentNeedsCompleted++;
        if(currentNeedsCompleted == todaysNeeds)
        {
            if(!TimeManager.Singleton.IsNight)
                StartEvening();
        }
    }

    public void StartDay()
    {
        Debug.Log("Day Starting!");
        List<CubertScreen> cubertScreens = ScreenManager.Singleton.GetCubertScreens();

        foreach(CubertScreen screen in cubertScreens)
        {
            todaysNeeds += screen.QueueNeeds();
        }

        // TimeManager.Singleton.CalculateTimeInterval(todaysNeeds);
        TimeManager.Singleton.FreezeTime(false);
        TimeManager.Singleton.SetDay();
    }

    public void AddNeed()
    {
        todaysNeeds++;
    }

    public void StartEvening()
    {
        Debug.Log("Evening Starting!");
        TimeManager.Singleton.SetNight();

        for(int i = 0; i < npcsInEvening.Count; i++)
        {
            npcQueue.Enqueue(npcsInEvening[i]);
        }
        GetNextNPC();

        if(TimeManager.Singleton.GetDay() == "Tuesday")
            GameObject.Find("TuesdayStuff").GetComponent<TuesdayStuff>().SpawnHiddenMuberts();
    }

    public bool PlaceCubertOnFrontDesk(GameObject cubert)
    {
        Debug.Log("Front Desk");
        if(currentNPC == null)
        {
            cubert.GetComponent<BoxCollider2D>().enabled = true;
            cubert.transform.SetParent(cubertHolder);
            cubert.transform.localPosition = Vector3.zero;
            cubert.transform.localScale = new Vector3(4f, 4f, 4f);
            return true;    
        }

        if(currentNPC.npcSO.cubert.name == cubert.name)
        {
            cubert.GetComponent<BoxCollider2D>().enabled = true;
            cubert.transform.SetParent(cubertHolder);
            cubert.transform.localPosition = Vector3.zero;
            cubert.transform.localScale = new Vector3(4f, 4f, 4f);

            npc.StartCubertDialogue();
            return true;
        }
        else
        {
            npc.StartWrongCubertDialogue();    
        }

        return false;
    }

    public GameObject GetCubertOnFrontDesk()
    {
        return(cubertHolder.transform.GetChild(0).gameObject);
    }

    public void EnableCubertDropOff()
    {
        cubertLocationCollider.enabled = true;
        customerCollider.enabled = false;
    }

    public void DisableCubertDropOff()
    {
        cubertLocationCollider.enabled = false;
        customerCollider.enabled = true;
    }
}
