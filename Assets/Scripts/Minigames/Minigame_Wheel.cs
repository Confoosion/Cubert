using UnityEngine;

public class Minigame_Wheel : MonoBehaviour
{
    [SerializeField] private GameObject minigameObj;
    [SerializeField] private GameObject stopMinigameButton;

    void Awake()
    {
        minigameObj.SetActive(false);
        stopMinigameButton.SetActive(false);
    }

    public void StartGame()
    {
        if(minigameObj.activeSelf) return;

        GlobalEvents.Singleton.PlayMinigame(true);

        minigameObj.SetActive(true);
        stopMinigameButton.SetActive(true);
    }

    public void StopGame()
    {
        GlobalEvents.Singleton.PlayMinigame(false);

        minigameObj.SetActive(false);
        stopMinigameButton.SetActive(false);
    }
}
