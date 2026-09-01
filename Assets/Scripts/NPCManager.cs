using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Singleton;

    [SerializeField] private List<NPCSO> npcList = new List<NPCSO>();
    [SerializeField] private List<NPCSO> npcsVisited = new List<NPCSO>();
    [SerializeField] private List<NPCSO> npcsFinished = new List<NPCSO>();

    void Awake()
    {
        if(Singleton == null) Singleton = this;
    }

    public void AddVisitor(NPCSO npc)
    {
        if(npcsVisited.Contains(npc) || npcsFinished.Contains(npc)) return;

        npcList.Remove(npc);
        npcsVisited.Add(npc);
    }

    public void AddFinishedVisitor(NPCSO npc)
    {
        if(npcsFinished.Contains(npc)) return;

        npcsVisited.Remove(npc);
        npcsFinished.Add(npc);
    }

    public NPCSO GetRandomNPC()
    {
        return(npcList[Random.Range(0, npcList.Count)]);
    }

    public bool IsNewNPC(NPCSO npc)
    {
        return(npcList.Contains(npc));
    }

    // public bool IsVisitedNPC(NPCSO npc)
    // {
    //     return(npcsVisited.Contains(npc));
    // }
}
