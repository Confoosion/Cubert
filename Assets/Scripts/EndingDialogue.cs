using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class EndingDialogue : MonoBehaviour
{
    [SerializeField] private float typingInterval = 0.25f;

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _sentenceText;
    [SerializeField] private GameObject dialogueObject;

    [SerializeField] private UnityEvent endEvent;

    private Queue<string> sentences = new Queue<string>();
    private Coroutine typeRoutine;
    private string currentSentence;

    void Awake()
    {
        HideDialogue();
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        _nameText.SetText("News Reporter");

        ShowDialogue();
        DisplayNextSentence();
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
        endEvent.Invoke();
        HideDialogue();
    }

    private void ShowDialogue()
    {
        dialogueObject.SetActive(true);
    }

    private void HideDialogue()
    {
        dialogueObject.SetActive(false);
    }
}
