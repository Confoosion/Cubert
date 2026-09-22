using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject dimBG;

    private float fadeSpeed = 1.5f;
    private Coroutine startRoutine = null;

    public void StartGame()
    {
        if(startRoutine == null)
        {
            startRoutine = StartCoroutine(StartGameAnim());
        }
        // SceneManager.LoadScene("Game");
    }

    IEnumerator StartGameAnim()
    {
        dimBG.SetActive(true);
        Image image = dimBG.GetComponent<Image>();
        Color currentColor = image.color;
        float currentAlpha = currentColor.a;

        while(currentAlpha < 1f)
        {
            currentAlpha += Time.deltaTime * fadeSpeed;
            currentColor.a = currentAlpha;

            image.color = currentColor;
            yield return null;
        }

        startRoutine = null;

        SceneManager.LoadScene("Friday");
        yield return null;
    }
}
