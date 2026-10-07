using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SafeEnding : MonoBehaviour
{
    [SerializeField] private GameObject dimBG;
    [SerializeField] private EndingDialogue endingDialogue;
    [SerializeField] private DialogueSO dialogue;
    [SerializeField] private DialogueSO trueDialogue;

    void Awake()
    {
        StartCoroutine(SafeEnd());
    }

    IEnumerator SafeEnd()
    {
        yield return FadeInScreen();
        yield return new WaitForSeconds(1);

        endingDialogue.StartDialogue(dialogue);
    }

    IEnumerator FadeInScreen()
    {
        dimBG.SetActive(true);
        Image image = dimBG.GetComponent<Image>();
        Color currentColor = image.color;
        float currentAlpha = currentColor.a;

        while(currentAlpha > 0f)
        {
            currentAlpha -= Time.deltaTime * 0.5f;
            currentColor.a = currentAlpha;

            image.color = currentColor;
            yield return null;
        }

        image.color = new Color(0f, 0f, 0f, 0f);
        yield return null;
    }

    public void EndScreen()
    {
        StartCoroutine(EndPause());
    }

    IEnumerator EndPause()
    {
        yield return new WaitForSeconds(2f);
        
        if(GlobalEvents.Singleton.ObtainedTrueEnd)
        {
            endingDialogue.StartDialogue(trueDialogue);
        }
        else
            FadeOut();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutScreen());
    }

    IEnumerator FadeOutScreen()
    {
        dimBG.SetActive(true);
        Image image = dimBG.GetComponent<Image>();
        Color currentColor = image.color;
        float currentAlpha = currentColor.a;

        while(currentAlpha < 1f)
        {
            currentAlpha += Time.deltaTime * 0.5f;
            currentColor.a = currentAlpha;

            image.color = currentColor;
            yield return null;
        }

        SceneManager.LoadScene("MainMenu");
        yield return null;
    }
}
