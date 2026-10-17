using UnityEngine;

public class OutlineRender : MonoBehaviour
{
    [SerializeField] private SpriteRenderer outline;

    void Awake()
    {
        outline.enabled = false;
    }

    private void OnMouseOver()
    {
        outline.enabled = true;
    }

    private void OnMouseExit()
    {
        outline.enabled = false;
    }
}
