using UnityEngine;

public class BouncyBall : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float followSpeed = 20f;
    [SerializeField] private float launchMultiplier = 1f;
    [SerializeField] private float maxLaunchSpeed = 15f;

    private bool isDragging = false;
    private Vector2 previousPosition;
    private Vector2 currentVelocityEstimate;

    [Header("Color Settings")]
    [SerializeField] private float cycleSpeed = 1f;
    [SerializeField] private float saturation = 1f;
    [SerializeField] private float value = 1f;
    [SerializeField] private float minSpeedRequirement = 0.5f;

    private float hue;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Camera cam;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    void Update()
    {
        if(isDragging) return;

        float speed = rb.linearVelocity.magnitude;

        if(speed > minSpeedRequirement)
        {
            hue += speed * cycleSpeed * Time.deltaTime;
            hue %= 1f;

            spriteRenderer.color = Color.HSVToRGB(hue, saturation, value);
        }
    }

    void FixedUpdate()
    {
        if(!isDragging) return;

        Vector2 targetPos = GetMouseWorldPosition();
        Vector2 newPos = Vector2.Lerp(rb.position, targetPos, followSpeed * Time.fixedDeltaTime);

        currentVelocityEstimate = (newPos - previousPosition) / Time.fixedDeltaTime;
        previousPosition = newPos;

        rb.MovePosition(newPos);
    }

    private void OnMouseDown()
    {
        isDragging = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        previousPosition = rb.position;
    }

    private void OnMouseUp()
    {
        if(isDragging)
        {
            isDragging = false;
            rb.bodyType = RigidbodyType2D.Dynamic;

            Vector2 launchVelocity = currentVelocityEstimate * launchMultiplier;
            launchVelocity = Vector2.ClampMagnitude(launchVelocity, maxLaunchSpeed);
            rb.linearVelocity = launchVelocity;
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(screenPos);
    }
}
