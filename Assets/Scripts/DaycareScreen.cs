using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DaycareScreen : MonoBehaviour
{
    public static DaycareScreen Singleton;
    void Awake() { if(Singleton == null) Singleton = this; }

    [SerializeField] private SpriteRenderer npcSkin;
    [SerializeField] private SpriteRenderer npcShirt;

    [SerializeField] private List<NPCSO> npcsInMorning = new List<NPCSO>();
    [SerializeField] private List<NPCSO> npcsInEvening = new List<NPCSO>();

    private Queue<NPCSO> npcQueue = new Queue<NPCSO>();
    private NPCSO currentNPC;

    [SerializeField] private NPC npc;

    [SerializeField] private Transform cubertHolder;

    [Header("Sounds")]
    [SerializeField] private AudioClip NPCEnterSound;

    void Start()
    {
        TimeManager.Singleton.ResetTime();
        TimeManager.Singleton.FreezeTime(true);
        // npcQueue.Clear();
        // npcsInMorning.Clear();

        // for(int i = 0; i < Random.Range(1, 4); i++)
        // {
        //     NPCSO newNPC = NPCManager.Singleton.GetRandomNPC();
        //     npcQueue.Enqueue(newNPC);
        //     npcsInMorning.Add(newNPC);
        //     if(newNPC.pickupSameDay)
        //     {
        //         npcsInEvening.Add(newNPC);
        //     }
        // }
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

            npcSkin.color = currentNPC.skinColor;
            npcShirt.color = currentNPC.shirtColor;
            npc.SetData(currentNPC);
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
    }

    public void StartDay()
    {
        Debug.Log("Day Starting!");
        List<CubertScreen> cubertScreens = ScreenManager.Singleton.GetCubertScreens();
        int needs = 0;

        foreach(CubertScreen screen in cubertScreens)
        {
            screen.QueueNeeds();
        }

        TimeManager.Singleton.CalculateTimeInterval(needs);
        TimeManager.Singleton.FreezeTime(false);
    }

    public void StartEvening()
    {
        Debug.Log("Evening Starting!");

        for(int i = 0; i < npcsInEvening.Count; i++)
        {
            npcQueue.Enqueue(npcsInEvening[i]);
        }
    }
}
