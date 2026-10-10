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
    private float rotationAccumulator;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(dragging || eventData.button != PointerEventData.InputButton.Left) return;

        dragging = true;
        activePointerID = eventData.pointerId;
        velocity = 0;
        lastDragFrame = Time.frameCount;
        lastAngle = AngleToPointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!dragging || eventData.pointerId != activePointerID) return;
        if(((Vector2)transform.position - ScreenToWorld(eventData)).sqrMagnitude < 0.0001f) return;

        float current = AngleToPointer(eventData);
        float delta = Mathf.DeltaAngle(lastAngle, current);
        lastAngle = current;
        lastDragFrame = Time.frameCount;

        float forward = Mathf.Max(0f, delta) * spinSensitivity;
        Rotate(forward);

        if(useMomentum && Time.deltaTime > 0f)
        {
            float target = Mathf.Min(forward / Time.deltaTime, maxSpinSpeed);
            velocity = Mathf.Lerp(velocity, target, 1f - Mathf.Exp(-velocitySmoothing * Time.deltaTime));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!dragging || eventData.pointerId != activePointerID) return;

        dragging = false;
        if(!useMomentum) velocity = 0f;
    }

    void Update()
    {
        if(dragging)
        {
            if(Time.frameCount - lastDragFrame > 1)
            {
                velocity = Mathf.Lerp(velocity, 0f, 1f - Mathf.Exp(-velocitySmoothing * Time.deltaTime));
                return; 
            }

            if(useMomentum && velocity > 0.5f)
            {
                Rotate(velocity * Time.deltaTime);
                velocity *= Mathf.Exp(-friction * Time.deltaTime);
            }
            else
            {
                velocity = 0f;
            }
        }
    }

    private void Rotate(float degrees)
    {
        if(degrees <= 0f) return;

        transform.Rotate(0f, 0f, degrees);
        
        rotationAccumulator += degrees;
        while(rotationAccumulator >= 360f)
        {
            rotationAccumulator -= 360f;
            onFullRotation?.Invoke();
        }
    }

    public void ResetWheel()
    {
        transform.rotation = Quaternion.identity;
        rotationAccumulator = 0f;
        velocity = 0f;
        dragging = false;
    }

    private Vector2 ScreenToWorld(PointerEventData eventData)
    {
        Camera cam = Camera.main;

        return(cam.ScreenToWorldPoint(eventData.position));
    }

    private float AngleToPointer(PointerEventData eventData)
    {
        Vector2 dir = ScreenToWorld(eventData) - (Vector2)transform.position;
        return(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}
