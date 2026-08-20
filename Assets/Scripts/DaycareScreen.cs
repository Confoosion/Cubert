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

    [SerializeField] private Animator npcAnimator;

    void Start()
    {
        if(npcsToday.Count < 1) return;
        npcSkin.color = npcsToday[0].skinColor;
        npcShirt.color = npcsToday[0].shirtColor;
        npc.SetData(npcsToday[0]);
        NPCEnter();
    }

    void Update()
    {
        if(npc.interacted)
        {
            npc.interacted = false;
            npc.StartIntroDialogue(dialogue);
        }
    }

    public void SpawnCubert()
    {
        Instantiate(npc.Data.cubert, cubertHolder.position, Quaternion.identity);
        ScreenManager.Singleton.AddCubert();
    }

    public void NPCEnter()
    {
        npcAnimator.SetBool("Enter", true);
    }

    public void NPCLeave()
    {
        npcAnimator.SetBool("Enter", false);
    }
}
