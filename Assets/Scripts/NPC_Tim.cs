using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class NPC_Tim : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private NPCSO _data;
    [SerializeField] private SpriteRenderer npcRenderer;
    public NPCSO Data => _data;

    private bool interacted;
    public bool Interacted => interacted;
    public void SetInteracted(bool interact) { interacted = interact; }

    [SerializeField] private Animator npcAnimator;
    [SerializeField] private AudioClip NPCLeaveSFX;

    private bool isBouncing;
    private float bounceElapsed;
    private float bounceDuration = 0.25f;
    private float bounceHeight = 0.5f;
    private Vector3 originalPosition;
    private bool hasOriginalPosition;

    void Awake()
    {
        npcAnimator.enabled = false;
    }

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
        StartPickUpDialogue();
    }

    public void StartPickUpDialogue()
    {
        NPCSO.NPCDialogue todaysDialogue = null;

        if(TimeManager.Singleton.GetDay() == "Friday" && _data.npcName == "Rose" && TimeManager.Singleton.IsNight)
            todaysDialogue = GetBranchDayDialogue();
        else
            todaysDialogue = GetCurrentDayDialogue();

        dialogue.StartDialogue(_data, todaysDialogue?.pickUpDialogue);

        if(_data.npcName == "Timothy" && TimeManager.Singleton.GetDay() == "Thursday" && TimeManager.Singleton.IsNight)
        {
            ScreenManager.Singleton.GetCubertScreen("Hubert").PerformHabit(Habit.HubertLeave);
        }
    }

    public void StartCubertDialogue()
    {
        NPCSO.NPCDialogue todaysDialogue = null;

        if(todaysDialogue == null)
            todaysDialogue = GetCurrentDayDialogue();

        dialogue.StartDialogue(_data, todaysDialogue.cubertDialogue);
    }

    public void StartWrongCubertDialogue()
    {
        dialogue.StartDialogue(_data, _data.wrongCubertDialogue);
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
        else if(HoldCubert.Singleton.HoldingCubert)
        {
            GameObject cube = HoldCubert.Singleton.HeldCubert.gameObject;
            DaycareScreen.Singleton.CubertHolder.GetComponent<CubertRoomSpot>().PutCubertInSpot(cube);
        }
    }

    public void NPCEnter()
    {
        npcRenderer.enabled = true;
        npcRenderer.sprite = _data.npcFrontSprite;

        npcAnimator.enabled = true;
        npcAnimator.SetBool("Enter", true);
    }

    public void NPCLeave()
    {
        npcAnimator.SetBool("Enter", false);
        npcRenderer.sprite = _data.npcBackSprite;
        StartCoroutine(Leaving());
    }

    IEnumerator Leaving()
    {
        AnimatorStateInfo animInfo = npcAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animInfo.length + 0.5f);

        SoundManager.Singleton?.PlaySFX(NPCLeaveSFX);
        npcRenderer.enabled = false;
    }

    public void LilBounce()
    {
        AnimatorStateInfo animInfo = npcAnimator.GetCurrentAnimatorStateInfo(0);
        if(animInfo.normalizedTime < 1f) return;

        // Debug.Log("DO LIL BOUNCE");
        if(!hasOriginalPosition)
        {
            originalPosition = transform.localPosition;
            hasOriginalPosition = true;
        }

        bounceElapsed = 0f;
        isBouncing = true;
    }

    void LateUpdate()
    {
        if(!isBouncing) return;

        bounceElapsed += Time.deltaTime;
        float t = bounceElapsed / bounceDuration;

        if(t >= 1f)
        {
            transform.localPosition = originalPosition;
            isBouncing = false;
            return;
        }

        float heightOffset = Mathf.Sin(t * Mathf.PI) * bounceHeight;
        transform.localPosition = originalPosition + new Vector3(0f, heightOffset, 0f);
    }
}
