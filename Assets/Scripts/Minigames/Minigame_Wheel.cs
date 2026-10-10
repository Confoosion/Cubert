using UnityEngine;

public class Minigame_Wheel : MonoBehaviour
{
    [SerializeField] private GameObject minigameObj;
    [SerializeField] private GameObject stopMinigameButton;

    public void StartGame()
    {
        if(minigameObj.activeSelf) return;

        minigameObj.SetActive(true);
        stopMinigameButton.SetActive(true);
    }
}
