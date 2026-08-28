using UnityEngine;

public class ShowerHead : MonoBehaviour
{
    [Header("Drag Settings")]
    [SerializeField] private float followSpeed = 20f;

    private Vector2 previousPosition;

    [SerializeField] private ParticleSystem waterParticles;
    private bool isHolding = false;

    private Rigidbody2D rb;
    private Camera cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        isHolding = true;
        waterParticles.Play();
    }

    private void OnMouseUp()
    {
        isHolding = false;
        waterParticles.Stop();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(!isHolding) return;

        Vector2 targetPos = GetMouseWorldPosition();
        Vector2 newPos = Vector2.Lerp(rb.position, targetPos, followSpeed * Time.fixedDeltaTime);

        previousPosition = newPos;

        rb.MovePosition(newPos);
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(screenPos);
    }
}
