using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MubertEnding : MonoBehaviour
{
    [SerializeField] private GameObject dimBG;
    [SerializeField] private EndingDialogue endingDialogue;
    [SerializeField] private DialogueSO dialogue;

    void Awake()
    {
        StartCoroutine(MubertEnd());
    }

    IEnumerator MubertEnd()
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
