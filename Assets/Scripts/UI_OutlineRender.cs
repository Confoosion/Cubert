using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_OutlineRender : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image outline;

    void Awake()
    {
        outline.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        outline.enabled = false;
    }
}
