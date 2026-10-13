using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject dimBG;

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;

    private float fadeSpeed = 1.5f;
    private Coroutine startRoutine = null;

    void Start()
    {
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

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

        SceneManager.LoadScene("Monday");
        // SceneManager.LoadScene("Tuesday");
        // SceneManager.LoadScene("Wednesday");
        yield return null;
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenCreditsMenu()
    {
        creditsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void CloseCreditsMenu()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void CloseGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
