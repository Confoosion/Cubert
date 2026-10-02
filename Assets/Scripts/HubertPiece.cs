using UnityEngine;

public class HubertPiece : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float launchForce = 20f;

    public void Launch()
    {
        rb.linearVelocity = Vector2.zero;
        Vector2 dir = DirectionFromAngle(launchAngle);
        rb.AddForce(dir * launchForce, ForceMode2D.Impulse);
    }

    private Vector2 DirectionFromAngle(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }
}
