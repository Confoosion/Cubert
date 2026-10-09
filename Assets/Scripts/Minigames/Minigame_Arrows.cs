using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public enum ArrowDirections { Up, Down, Left, Right }

public class Minigame_Arrows : MonoBehaviour
{
    [SerializeField] private GameObject minigameObj;
    [SerializeField] private GameObject stopMinigameButton;

    [SerializeField] private Transform redArrow;
    [SerializeField] private ArrowButton[] arrowButtons;

    private ArrowDirections currentDirection;
    public ArrowDirections CurrentDirection => currentDirection;

    private int currentScore;
    public void AddScore() => currentScore++;

    void Awake()
    {
        foreach(ArrowButton arrow in arrowButtons)
        {
            arrow.Initialize(this);
        }

        minigameObj.SetActive(false);
        stopMinigameButton.SetActive(false);
    }

    public void StartGame()
    {
        if(minigameObj.activeSelf) return;

        currentScore = 0;
        minigameObj.SetActive(true);
        stopMinigameButton.SetActive(true);
    }

    public void ChangeArrows()
    {
        RandomizeDirection();

        if(currentScore > 4)
        {
            RandomizeArrows();
        }
    }

    private void ResetDirections()
    {
        currentDirection = ArrowDirections.Up;
        for(int i = 0; i < arrowButtons.Length; i++)
        {
            arrowButtons[i].SetArrow((ArrowDirections)Enum.GetValues(typeof(ArrowDirections)).GetValue(i));
        }
    }

    private void RandomizeDirection()
    {
        currentDirection = (ArrowDirections)Enum.GetValues(typeof(ArrowDirections)).GetValue(UnityEngine.Random.Range(0, 4));
        switch(currentDirection)
        {
            case ArrowDirections.Up:
                redArrow.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case ArrowDirections.Down:
                redArrow.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case ArrowDirections.Left:
                redArrow.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case ArrowDirections.Right:
                redArrow.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }
    }

    private void RandomizeArrows()
    {
        List<int> options = new List<int>() { 1, 2, 3, 4 };
        foreach(ArrowButton arrow in arrowButtons)
        {
            int option = options[UnityEngine.Random.Range(0, options.Count)];
            options.Remove(option);

            arrow.SetArrow((ArrowDirections)Enum.GetValues(typeof(ArrowDirections)).GetValue(option - 1));
        }
    }

    public void StopGame()
    {
        minigameObj.SetActive(false);
        stopMinigameButton.SetActive(false);
    }
}
