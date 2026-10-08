using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class StopMinigameButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent buttonPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(gameObject.activeSelf)
        {
            buttonPressed?.Invoke();
        }
    }
}
