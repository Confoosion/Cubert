using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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
        if(Enum.TryParse(TimeManager.Singleton.GetDay(), out Day currentDay))
        {
            var entry = _data.npcDialogue.FirstOrDefault(d => d.day == currentDay);
            return entry;
        }

        return null;
    }

    private NPCSO.NPCDialogue GetBranchDayDialogue()
    {
        if(Enum.TryParse(TimeManager.Singleton.GetDay(), out Day currentDay))
        {
            var matches = _data.branchDialogue.Where(d => d.day == currentDay);

            if(currentDay == Day.Friday && _data.npcName == "Rose" && TimeManager.Singleton.IsNight)
                return matches.Skip(1).FirstOrDefault();
            return matches.FirstOrDefault();
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

        if(_data.npcName == "Calvin" && TimeManager.Singleton.GetDay() == "Thursday" && GlobalEvents.Singleton.MubertGone)
        {
            ScreenManager.Singleton.GetCubertScreen("Mubert")?._Cubert.GetComponent<MubertStuff>().Appear();
        }
    }

    public void StartPickUpDialogue()
    {
        NPCSO.NPCDialogue todaysDialogue = null;

        if(TimeManager.Singleton.GetDay() == "Friday" && _data.npcName == "Rose" && TimeManager.Singleton.IsNight)
            todaysDialogue = GetBranchDayDialogue();
        else
            todaysDialogue = GetCurrentDayDialogue();

        dialogue.StartDialogue(_data.npcName, todaysDialogue?.pickUpDialogue);

        if(_data.npcName == "Timothy" && TimeManager.Singleton.GetDay() == "Thursday" && TimeManager.Singleton.IsNight)
        {
            ScreenManager.Singleton.GetCubertScreen("Hubert").PerformHabit(Habit.HubertLeave);
        }
    }

    public void StartCubertDialogue()
    {
        NPCSO.NPCDialogue todaysDialogue = null;

        string day = TimeManager.Singleton.GetDay();
        if(day == "Wednesday")
        {
            if(_data.npcName == "Anna" && GlobalEvents.Singleton.AnnaAngry)
                todaysDialogue = GetBranchDayDialogue();
            else if(_data.npcName == "Timothy" && GlobalEvents.Singleton.OubertUncomfortable)
                todaysDialogue = GetBranchDayDialogue();      
        }
        else if(day == "Friday")
        {
            if(_data.npcName == "Rose" && (GlobalEvents.Singleton.FubertMakeupRuined || TimeManager.Singleton.IsNight))
            {
                todaysDialogue = GetBranchDayDialogue();
            }
        }

        if(todaysDialogue == null)
            todaysDialogue = GetCurrentDayDialogue();

        dialogue.StartDialogue(_data.npcName, todaysDialogue.cubertDialogue);
    }

    public void StartWrongCubertDialogue()
    {
        dialogue.StartDialogue(_data.npcName, _data.wrongCubertDialogue);
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
