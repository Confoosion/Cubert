using UnityEngine;

public class CubertLogo : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private SpriteRenderer monitor;
    [SerializeField] private Color[] monitorColors;
    private int colorIndex = 0;
    private Rigidbody2D rb;
    private Vector2 direction;
    private int lastHitStep = -1;
    Vector2 lastNormal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        float angle = Random.Range(0f, Mathf.PI * 2f);
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    void Start()
    {
        rb.linearVelocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = Vector2.zero;
        int contactCount = collision.contactCount;

        for(int i = 0; i < contactCount; i++)
        {
            normal += collision.GetContact(i).normal;
        }

        if(normal.sqrMagnitude < 0.0001f) return;
        normal.Normalize();

        int step = Mathf.RoundToInt(Time.fixedTime / Time.fixedDeltaTime);
        if(step == lastHitStep && Vector2.Dot(normal, lastNormal) < 0.5f)
        {
            colorIndex = (colorIndex + 1) % monitorColors.Length;
            monitor.color = monitorColors[colorIndex];
        }
        lastHitStep = step;
        lastNormal = normal;

        direction = Vector2.Reflect(direction, normal);
        rb.linearVelocity = direction * speed;
    }
}
