using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private float typingInterval = 0.25f;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _sentenceText;
    [SerializeField] private GameObject dialogueObject;

    private DialogueType currentDialogueType;
    private Queue<string> sentences = new Queue<string>();
    private Coroutine typeRoutine;
    private string currentSentence;

    private int INTRO_CUBERT_INDEX = 1;
    private int sentenceIndex = 0;

    public bool IsDialogueOpen => dialogueObject.activeSelf;

    void Awake()
    {
        HideDialogue();
    }

    public void StartDialogue(string npcName, DialogueSO dialogue)
    {
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        currentDialogueType = dialogue.dialogueType;
        sentenceIndex = 0;

        _nameText.SetText(npcName);
        ShowDialogue();
        DisplayNextSentence();
        DaycareScreen.Singleton.DisableCubertDropOff();
    }

    public void DisplayNextSentence()
    {
        if(typeRoutine != null)
        {
            StopCoroutine(typeRoutine);
            typeRoutine = null;
            _sentenceText.SetText(currentSentence);
            return;
        }

        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();

        typeRoutine = StartCoroutine(TypeSentence());

        if(currentDialogueType == DialogueType.Intro && sentenceIndex == INTRO_CUBERT_INDEX)
        {
            DaycareScreen.Singleton.SpawnCubert();
        }
        sentenceIndex++;
    }

    IEnumerator TypeSentence()
    {
        _sentenceText.text = "";
        foreach(char c in currentSentence.ToCharArray())
        {
            _sentenceText.text += c;
            yield return new WaitForSeconds(typingInterval);
        }

        typeRoutine = null;
    }

    private void EndDialogue()
    {
        if(typeRoutine != null)
            StopCoroutine(typeRoutine);
        
        HideDialogue();

        if(currentDialogueType is DialogueType.Intro or DialogueType.Cubert)
        {
            DaycareScreen.Singleton.NPCLeave();
            
            if(currentDialogueType is DialogueType.Cubert)
            {
                ScreenManager.Singleton.RemoveCubert();    
            }
        }
        else if(currentDialogueType is DialogueType.PickUp)
        {
            if(TimeManager.Singleton.GetDay() == "Wednesday" && DaycareScreen.Singleton.CurrentNPC.npcSO.npcName == "Anna")
            {
                TimeManager.Singleton.StartAnnaTimer();
            }

            DaycareScreen.Singleton.EnableCubertDropOff();
        }
    }

    private void ShowDialogue()
    {
        dialogueObject.SetActive(true);
        ScreenManager.Singleton.HideArrows();
    }

    private void HideDialogue()
    {
        dialogueObject.SetActive(false);
        ScreenManager.Singleton.EnableArrows();
    }
}
