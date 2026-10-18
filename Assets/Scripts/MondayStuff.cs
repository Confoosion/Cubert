using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MondayStuff : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private DialogueSO introDialogue;
    [SerializeField] private GameObject nextSentenceHitbox;
    [SerializeField] private GameObject clickAnywhereText;

    private Queue<string> sentences = new Queue<string>();
    private int sentenceIndex = 0;
    private Coroutine fadeRoutine;

    void Awake()
    {
        introText.color = new Color(1f, 1f, 1f, 0f);
        introText.enabled = true;
    }

    public void DisplayIntroText()
    {
        sentences.Clear();

        foreach(string sentence in introDialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        sentenceIndex = 0;
        DisplayNextSentence();
        nextSentenceHitbox.SetActive(true);

        clickAnywhereText.SetActive(true);
    }

    IEnumerator FadeInText(string dialogueText)
    {
        if(sentenceIndex > 0)
        {
            yield return FadeOutText();    
        }
        else
        {
            yield return new WaitForSeconds(2.5f);
        }

        introText.SetText(dialogueText);

        Color c = introText.color;
        float alpha = c.a;

        while(alpha < 1f)
        {
            alpha += Time.deltaTime * 2f;
            introText.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        introText.color = new Color(1f, 1f, 1f, 1f);
        fadeRoutine = null;
        yield return null;
    }

    IEnumerator FadeOutText()
    {
        if(clickAnywhereText.activeSelf)
            clickAnywhereText.SetActive(false);

        Color c = introText.color;
        float alpha = c.a;

        while(alpha > 0f)
        {
            alpha -= Time.deltaTime * 2f;
            introText.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        introText.color = new Color(1f, 1f, 1f, 0f);

        yield return null;
    }

    public void DisplayNextSentence()
    {
        if(fadeRoutine != null)
        {
            return;
        }

        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string currentSentence = sentences.Dequeue();

        fadeRoutine = StartCoroutine(FadeInText(currentSentence));

        sentenceIndex++;
    }

    private void EndDialogue()
    {
        StartCoroutine(EndPart());
    }

    IEnumerator EndPart()
    {
        nextSentenceHitbox.SetActive(false);
        yield return FadeOutText();
        DaycareScreen.Singleton.PlayIntroTransition();
    }
}