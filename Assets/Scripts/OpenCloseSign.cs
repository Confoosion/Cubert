using UnityEngine;

public class OpenCloseSign : MonoBehaviour
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private SpriteRenderer signRenderer;

    [SerializeField] private bool isOpen;
    public bool IsOpen => isOpen;

    void Awake()
    {
        FlipToClosed();
    }

    private void FlipToOpen()
    {        
        isOpen = true;
        signRenderer.sprite = closedSprite;
    }

    private void FlipToClosed()
    {
        isOpen = false;
        signRenderer.sprite = openSprite;
    }

    void OnMouseDown()
    {
        Task task = TaskManager.Singleton.CurrentTask;
        if(isOpen && task == Task.Close)
        {
            FlipToClosed();
            TaskManager.Singleton.CompleteTask();
        }
        else if(!isOpen && task == Task.Open)
        {
            FlipToOpen();
            TaskManager.Singleton.CompleteTask();
        }
    }
}
