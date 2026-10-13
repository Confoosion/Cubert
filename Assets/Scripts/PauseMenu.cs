using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;

    private InputSystem_Actions playerControls;

    private bool isPaused = false;
    public bool IsPaused => isPaused;

    void Awake()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);

        playerControls = new InputSystem_Actions();
    }

    void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Pause.performed += OnPause;
    }

    void OnDisable()
    {
        playerControls.Player.Pause.performed -= OnPause;
        playerControls.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if(!isPaused)
        {
            Debug.Log("Pausing");
            isPaused = true;
            pauseMenu.SetActive(true);
            TimeManager.Singleton.FreezeTime(true);
            Time.timeScale = 0f;
        }
        else
        {
            Resume();
        }
    }

    public void Resume()
    {
        Debug.Log("Resuming");
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        isPaused = false;
        TimeManager.Singleton.FreezeTime(false);
        Time.timeScale = 1f;
    }

    public void OpenSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
