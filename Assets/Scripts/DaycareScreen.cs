using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    }

    [SerializeField] private List<NPCPerson> npcsInMorning = new List<NPCPerson>();
    [SerializeField] private List<NPCPerson> npcsInEvening = new List<NPCPerson>();
    // [SerializeField] private List<NPCSO> npcsInMorning = new List<NPCSO>();
    // [SerializeField] private List<NPCSO> npcsInEvening = new List<NPCSO>();

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

    void Start()
    {
        TimeManager.Singleton.ResetTime();
        TimeManager.Singleton.FreezeTime(true);
        
        for(int i = 0; i < npcsInMorning.Count; i++)
        {
            npcQueue.Enqueue(npcsInMorning[i]);
        }

        GetNextNPC();
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

            npc.SetData(currentNPC.npcSO, currentNPC.purpose);
            NPCEnter();
        }
        else if(TimeManager.Singleton.IsAM)
        {
            StartDay();
        }
    }

    public void NPCEnter()
    {
        npc.SetInteracted(false);
        npc.NPCEnter();
        SoundManager.Singleton?.PlaySFX(NPCEnterSound);
        ScreenManager.Singleton.SetNPCScreen(true);

        if(npc.Data.npcName == "Timothy" && SceneManager.GetActiveScene().name == "Wednesday")
        {
            GameObject.Find("WednesdayStuff").GetComponent<WednesdayStuff>().HubertLeave();
        }
    }

    public void NPCLeave()
    {
        npc.NPCLeave();
        ScreenManager.Singleton.SetNPCScreen(false);

        if(!TimeManager.Singleton.IsMorning)
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

        if(SceneManager.GetActiveScene().name == "Tuesday")
        {
            GameObject.Find("TuesdayStuff").GetComponent<TuesdayStuff>().SpawnHiddenMuberts();
        }
        else if(SceneManager.GetActiveScene().name == "Wednesday")
        {
            // GameObject.Find("TuesdayStuff").GetComponent<TuesdayStuff>().SpawnHiddenMuberts();

        }
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
        
        return false;
    }

    public GameObject GetCubertOnFrontDesk()
    {
        return(cubertHolder.transform.GetChild(0).gameObject);
    }
}
