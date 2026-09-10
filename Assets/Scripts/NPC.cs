using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private NPCSO _data;
    public NPCSO Data => _data;
    [SerializeField] private Purpose _purpose;
    [SerializeField] private SpriteRenderer npcRenderer;

    private bool interacted;
    public bool Interacted => interacted;
    public void SetInteracted(bool interact) { interacted = interact; }

    [SerializeField] private Animator npcAnimator;

    private NPCSO.NPCDialogue GetCurrentDayDialogue()
    {
        if(Enum.TryParse(SceneManager.GetActiveScene().name, out Day currentDay))
        {
            var entry = _data.npcDialogue.FirstOrDefault(d => d.day == currentDay);
            return entry;
        }

        return null;
    }

    private void DisplayDialogue()
    {
        if(_purpose is Purpose.DropOff)
        {
            StartIntroDialogue();
        }
        else
        {
            StartPickUpDialogue();
        }
    }

    public void StartIntroDialogue()
    {
        var todaysDialogue = GetCurrentDayDialogue();
        dialogue.StartDialogue(_data.npcName, todaysDialogue?.introDialogue);
    }

    public void StartPickUpDialogue()
    {
        var todaysDialogue = GetCurrentDayDialogue();
        dialogue.StartDialogue(_data.npcName, todaysDialogue?.pickUpDialogue);
    }

    public void StartCubertDialogue()
    {
        var todaysDialogue = GetCurrentDayDialogue();
        dialogue.StartDialogue(_data.npcName, todaysDialogue?.cubertDialogue);
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

    public void SetData(NPCSO data, Purpose purpose)
    {
        _data = data;
        _purpose = purpose;
    }

    public void NPCEnter()
    {
        npcAnimator.SetBool("Enter", true);
        npcRenderer.sprite = _data.npcFrontSprite;
    }

    public void NPCLeave()
    {
        npcAnimator.SetBool("Enter", false);
        npcRenderer.sprite = _data.npcBackSprite;
    }
}
