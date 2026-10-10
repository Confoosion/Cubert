using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Minigame_Mash : MonoBehaviour
{
    [SerializeField] private GameObject minigameObj;
    [SerializeField] private GameObject stopMinigameButton;

    [SerializeField] private Slider slider;

    [SerializeField] private UnityEvent onCompleteBar;

    private float decreaseAmount = 0.001f;
    private float increaseAmount = 0.2f;

    void Awake()
    {
        slider.value = 0f;

        minigameObj.SetActive(false);
        stopMinigameButton.SetActive(false);
        slider.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        if(minigameObj.activeSelf) return;

        GlobalEvents.Singleton.PlayMinigame(true);
        minigameObj.SetActive(true);
        stopMinigameButton.SetActive(true);
        slider.gameObject.SetActive(true);
    }

    public void StopGame()
    {
        GlobalEvents.Singleton.PlayMinigame(false);
        minigameObj.SetActive(false);
        stopMinigameButton.SetActive(false);
        slider.gameObject.SetActive(false);
    }

    public void ButtonPressed()
    {
        StartGame();
        IncreaseBar();
    }

    private void IncreaseBar()
    {
        slider.value += increaseAmount;
        if(slider.value >= 1f)
        {
            slider.value = 0f;
            onCompleteBar?.Invoke();
        }
    }

    void Update()
    {
        if(minigameObj.activeSelf)
            slider.value = Mathf.Max(0f, slider.value - decreaseAmount);
    }
}
