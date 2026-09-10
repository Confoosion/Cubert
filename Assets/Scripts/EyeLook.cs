using UnityEngine;

public class EyeLook : MonoBehaviour
{
    [SerializeField] private Transform pupil;
    [SerializeField] private Transform eyeCenter;
    [SerializeField] private float xRadius = 0.2f;
    [SerializeField] private float yRadius = 0.2f;

    [SerializeField] private bool inverseEyes = false;

    private Camera cam;

    private Vector3 cubertScale;

    private void Awake()
    {
        cam = Camera.main;
        cubertScale = transform.parent.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(xRadius <= 0f && yRadius <= 0f)
            return;

        cubertScale = transform.parent.localScale;

        float scaledXRadius = xRadius * cubertScale.x;
        float scaledYRadius = yRadius * cubertScale.y;

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = eyeCenter.position.z;

        Vector2 direction = (Vector2)mouseWorldPos - (Vector2)eyeCenter.position;
        
        Vector2 normalized = new Vector2((scaledXRadius > 0f) ? direction.x / scaledXRadius : 0f, (scaledYRadius > 0f) ? direction.y / scaledYRadius : 0f);
        Vector2 clampedNormalized = Vector2.ClampMagnitude(inverseEyes ? -normalized : normalized, 1f);
        Vector2 clampedOffset = new Vector2(clampedNormalized.x * scaledXRadius, clampedNormalized.y * scaledYRadius);

        pupil.position = (Vector2)eyeCenter.position + clampedOffset;
    }

    private void OnDrawGizmos()
    {
        if(eyeCenter == null) return;

        Vector3 gizmoScale = transform.parent.localScale;
        float scaledXRadius = xRadius * gizmoScale.x;
        float scaledYRadius = yRadius * gizmoScale.y;

        Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(eyeCenter.position, radius);

        Vector3 prevPoint = eyeCenter.position + new Vector3(scaledXRadius, 0f, 0f);
        for(int i = 0; i <= 32; i++)
        {
            float angle = (i / 32f) * Mathf.PI * 2f;
            Vector3 point = eyeCenter.position + new Vector3(Mathf.Cos(angle) * scaledXRadius, Mathf.Sin(angle) * scaledYRadius, 0f);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}
