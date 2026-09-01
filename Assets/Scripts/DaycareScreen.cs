using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DaycareScreen : MonoBehaviour
{
    public static DaycareScreen Singleton;
    void Awake() { if(Singleton == null) Singleton = this; }

    [SerializeField] private SpriteRenderer npcSkin;
    [SerializeField] private SpriteRenderer npcShirt;

    [SerializeField] private List<NPCSO> npcsToday = new List<NPCSO>();
    private Queue<NPCSO> npcQueue = new Queue<NPCSO>();
    private NPCSO currentNPC;

    [SerializeField] private NPC npc;

    [SerializeField] private Transform cubertHolder;

    void Start()
    {
        TimeManager.Singleton.FreezeTime(true);
        
        for(int i = 0; i < Random.Range(1, 6); i++)
        {
            NPCSO newNPC = NPCManager.Singleton.GetRandomNPC();
            npcQueue.Enqueue(newNPC);
            npcsToday.Add(newNPC);
        }

        // if(npcsToday.Count < 1) return;
        currentNPC = npcQueue.Dequeue();
        
        npcSkin.color = currentNPC.skinColor;
        npcShirt.color = currentNPC.shirtColor;
        npc.SetData(currentNPC);
        npc.NPCEnter();
    }

    void Update()
    {
        // if(npc.Interacted)
        // {
        //     npc.StartIntroDialogue();
        // }
    }

    public void SpawnCubert()
    {
        GameObject cubert = Instantiate(npc.Data.cubert, cubertHolder.position, Quaternion.identity);
        cubert.name = npc.Data.cubert.name;
        ScreenManager.Singleton.AddCubert(npc.Data.cubert);
    }

    public void NPCEnter()
    {
        npc.SetInteracted(false);
        npc.NPCEnter();
    }

    public void NPCLeave()
    {
        npc.NPCLeave();
    }

    public void StartMorning()
    {
        List<CubertScreen> cubertScreens = ScreenManager.Singleton.GetCubertScreens();

        foreach(CubertScreen screen in cubertScreens)
        {
            for(int i = 0; i < Random.Range(screen.MinNeeds, screen.MaxNeeds + 1); i++)
            {
                screen.QueueNeed();
            }
        }

        TimeManager.Singleton.FreezeTime(false);
    }
}
