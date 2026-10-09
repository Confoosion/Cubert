using UnityEngine;
using UnityEngine.Events;

public class ArrowButton : MonoBehaviour
{
    [SerializeField] private ArrowDirections direction;

    [SerializeField] private UnityEvent onCorrect;
    [SerializeField] private UnityEvent onWrong;

    private Minigame_Arrows arrowMG;

    public void Initialize(Minigame_Arrows mg)
    {
        arrowMG = mg;
    }

    public void SetArrow(ArrowDirections dir)
    {
        direction = dir;
        switch(direction)
        {
            case ArrowDirections.Up:
                transform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            case ArrowDirections.Down:
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case ArrowDirections.Left:
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case ArrowDirections.Right:
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }
    }

    void OnMouseDown()
    {
        // Debug.Log("Clicking");
        if(arrowMG.CurrentDirection == direction)
        {
            onCorrect?.Invoke();
        }
        else
        {
            onWrong?.Invoke();
        }
    }
}
