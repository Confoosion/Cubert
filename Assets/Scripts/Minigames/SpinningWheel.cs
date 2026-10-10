using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpinningWheel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private float spinSensitivity = 0.9f;
    [SerializeField] private bool useMomentum = true;
    [SerializeField] private float friction = 2.5f;
    [SerializeField] private float maxSpinSpeed = 360f;
    [SerializeField] private float velocitySmoothing = 15f;

    public UnityEvent onFullRotation;

    private bool dragging;
    private int activePointerID;
    private int lastDragFrame;
    private float lastAngle;
    private float velocity;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(dragging || eventData.button != PointerEventData.InputButton.Left) return;

        
    }

    void Update()
    {
        
    }
}
