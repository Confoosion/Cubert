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

    [SerializeField] private Dialogue dialogue;
    [SerializeField] private NPC npc;

    [SerializeField] private Transform cubertHolder;

    void Start()
    {
        if(npcsToday.Count < 1) return;
        npcSkin.color = npcsToday[0].skinColor;
        npcShirt.color = npcsToday[0].shirtColor;
        npc.SetData(npcsToday[0]);
        npc.NPCEnter();
    }

    void Update()
    {
        if(npc.Interacted)
        {
            npc.SetInteracted(false);
            npc.StartIntroDialogue(dialogue);
        }
    }

    public void SpawnCubert()
    {
        GameObject cubert = Instantiate(npc.Data.cubert, cubertHolder.position, Quaternion.identity);
        cubert.name = npc.Data.cubert.name;
        ScreenManager.Singleton.AddCubert(npc.Data.cubert);
    }

    public void NPCEnter()
    {
        npc.NPCEnter();
    }

    public void NPCLeave()
    {
        npc.NPCLeave();
    }
}
