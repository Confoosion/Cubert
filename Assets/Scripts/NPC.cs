using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private NPCSO _data;
    public NPCSO Data => _data;
    [SerializeField] private Sprite npcSprite;
    public Sprite GetSprite() { return npcSprite; }

    private bool interacted;
    public bool Interacted => interacted;
    public void SetInteracted(bool interact) { interacted = interact; }

    [SerializeField] private Animator npcAnimator;

    private void DisplayDialogue()
    {
        if(NPCManager.Singleton.IsNewNPC(_data))
        {
            StartIntroDialogue();
            NPCManager.Singleton.AddVisitor(_data);
        }
        else
        {
            StartPickUpDialogue();
        }
    }

    public void StartIntroDialogue()
    {
        dialogue.StartDialogue(_data.npcName, _data.introDialogue);
    }

    public void StartPickUpDialogue()
    {
        dialogue.StartDialogue(_data.npcName, _data.pickUpDialogue);
    }

    public void StartCubertDialogue()
    {
        dialogue.StartDialogue(_data.npcName, _data.cubertDialogue);
    }

    void OnMouseDown()
    {
        AnimatorStateInfo animInfo = npcAnimator.GetCurrentAnimatorStateInfo(0);

        if(!interacted && !dialogue.IsDialogueOpen && animInfo.normalizedTime >= 1.0f)
        {
            interacted = true;
            DisplayDialogue();
        }
        else if(dialogue.IsDialogueOpen)
            dialogue.DisplayNextSentence();
    }

    public void SetData(NPCSO data)
    {
        _data = data;
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
