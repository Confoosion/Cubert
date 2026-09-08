using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Purpose { DropOff, PickUp }

public class DaycareScreen : MonoBehaviour
{
    public static DaycareScreen Singleton;
    void Awake() { if(Singleton == null) Singleton = this; }

    [SerializeField] private SpriteRenderer npcSkin;
    [SerializeField] private SpriteRenderer npcShirt;

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

    [SerializeField] private NPC npc;

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

            npcSkin.color = currentNPC.npcSO.skinColor;
            npcShirt.color = currentNPC.npcSO.shirtColor;
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
    }

    public void NPCLeave()
    {
        npc.NPCLeave();
        ScreenManager.Singleton.SetNPCScreen(false);

        if(!TimeManager.Singleton.IsMorning)
        {
            StartCoroutine(SpawnNightNPC());
        }
    }

    IEnumerator SpawnNightNPC()
    {
        yield return new WaitForSeconds(2f);
        
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

        TimeManager.Singleton.CalculateTimeInterval(todaysNeeds);
        TimeManager.Singleton.FreezeTime(false);
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
    }

    public bool PlaceCubertOnFrontDesk(GameObject cubert)
    {
        Debug.Log("Front Desk");
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
