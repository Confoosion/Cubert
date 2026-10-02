using UnityEngine;

public class HubertPiece : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float launchForce = 20f;

    [SerializeField] private AudioClip flesh;
    private AudioSource hubertSource;
    private bool isLaunching = false;

    private float soundCooldown = 0.05f;
    private static float lastSoundTime = float.NegativeInfinity;

    public void Launch(AudioSource source)
    {
        hubertSource = source;

        rb.linearVelocity = Vector2.zero;
        Vector2 dir = DirectionFromAngle(launchAngle);
        rb.AddForce(dir * launchForce, ForceMode2D.Impulse);

        isLaunching = true;
    }

    private Vector2 DirectionFromAngle(float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(hubertSource == null || !isLaunching) return;
        if(Time.time - lastSoundTime < soundCooldown) return;

        lastSoundTime = Time.time;
        hubertSource.PlayOneShot(flesh);
    }
}
