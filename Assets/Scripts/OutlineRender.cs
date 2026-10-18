using UnityEngine;

public class OutlineRender : MonoBehaviour
{
    [SerializeField] private SpriteRenderer outline;

    private bool disableOutline = false;
    private bool clicked = false;

    void Awake()
    {
        outline.enabled = false;
    }

    private void OnMouseOver()
    {
        if(!disableOutline)
            outline.enabled = true;
    }

    private void OnMouseDown()
    {
        clicked = true;
        disableOutline = true;
        outline.enabled = false;
    }

    private void OnMouseUp()
    {
        clicked = false;
        disableOutline = false;
    }

    private void OnMouseExit()
    {
        if(!clicked)
            disableOutline = false;
        outline.enabled = false;
    }
}
