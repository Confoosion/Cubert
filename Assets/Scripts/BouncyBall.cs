using UnityEngine;

public class BouncyBall : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float dragTimeRequired = 0.15f;
    [SerializeField] private float dragDistanceRequired = 0.1f;
    [SerializeField] private float followSpeed = 20f;
    [SerializeField] private float launchMultiplier = 1f;
    [SerializeField] private float maxLaunchSpeed = 15f;

    private bool isPressed = false;
    private bool isDragging = false;
    public bool IsDragging => isDragging;
    private float pressTime = 0f;
    private Vector2 pressStartPos;
    private Vector2 previousPosition;
    private Vector2 currentVelocityEstimate;
    private Vector3 ballScale;

    [Header("Color Settings")]
    [SerializeField] private float cycleSpeed = 1f;
    [SerializeField] private float saturation = 1f;
    [SerializeField] private float value = 1f;
    [SerializeField] private float minSpeedRequirement = 0.5f;

    [Header("Sound Settings")]
    [SerializeField] private AudioClip bounceSound;

    private float hue;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Camera cam;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        ballScale = transform.localScale;
    }

    void Update()
    {
        if(isDragging) return;

        if(isPressed && pressTime <= dragTimeRequired)
        {
            pressTime += Time.deltaTime;
            float movedDist = Vector2.Distance(GetMouseWorldPosition(), pressStartPos);

            if(pressTime > dragTimeRequired || movedDist > dragDistanceRequired)
            {
                BeginDrag();
            }
        }

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

    private void BeginDrag()
    {
        HoldCubert.Singleton.DropBall();
        transform.localScale = ballScale;

        isDragging = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        previousPosition = rb.position;
    }

    private void OnMouseDown()
    {
        isPressed = true;
        isDragging = false;
        pressTime = 0f;
        pressStartPos = GetMouseWorldPosition();
        previousPosition = rb.position;
    }

    private void OnMouseUp()
    {
        isPressed = false;

        if(isDragging)
        {
            isDragging = false;
            rb.bodyType = RigidbodyType2D.Dynamic;

            Vector2 launchVelocity = currentVelocityEstimate * launchMultiplier;
            launchVelocity = Vector2.ClampMagnitude(launchVelocity, maxLaunchSpeed);
            rb.linearVelocity = launchVelocity;
        }
        else
        {
            HoldCubert.Singleton.GrabBall(gameObject);
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(screenPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Transform parent = transform.parent;
        if(ScreenManager.Singleton.CurrentScreen == parent)
            SoundManager.Singleton.PlaySFX(bounceSound);
    }
}
