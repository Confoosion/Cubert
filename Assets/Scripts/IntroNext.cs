using UnityEngine;
using UnityEngine.EventSystems;

public class IntroNext : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MondayStuff mondayStuff;
    public void OnPointerClick(PointerEventData eventData)
    {
        mondayStuff.DisplayNextSentence();
    }
}
