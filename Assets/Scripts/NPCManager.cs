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
        NPCSO random = npcList[Random.Range(0, npcList.Count)];
        npcList.Remove(random);
        return(random);
    }

    public bool IsNewNPC(NPCSO npc)
    {
        return(!npcsVisited.Contains(npc));
    }

    // public bool IsVisitedNPC(NPCSO npc)
    // {
    //     return(npcsVisited.Contains(npc));
    // }
}
