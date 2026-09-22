using UnityEngine;

public class CubertLogo : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private SpriteRenderer monitor;
    [SerializeField] private Color[] monitorColors;
    private int colorIndex = 0;
    private Rigidbody2D rb;
    private Vector2 direction;

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

        if(contactCount == 2)
        {
            colorIndex = (colorIndex + 1) % monitorColors.Length;
            monitor.color = monitorColors[colorIndex];
        }

        if(contactCount > 0)
        {
            normal /= contactCount;
        }

        if(normal.sqrMagnitude < 0.0001f)
        {
            return;
        }

        normal.Normalize();

        direction = Vector2.Reflect(direction, normal);
        rb.linearVelocity = direction * speed;
    }
}
