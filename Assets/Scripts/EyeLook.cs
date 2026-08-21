using UnityEngine;

public class EyeLook : MonoBehaviour
{
    [SerializeField] private Transform pupil;
    [SerializeField] private Transform eyeCenter;
    [SerializeField] private float xRadius = 0.2f;
    [SerializeField] private float yRadius = 0.2f;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = eyeCenter.position.z;

        Vector2 direction = (Vector2)mouseWorldPos - (Vector2)eyeCenter.position;
        
        Vector2 normalized = new Vector2(direction.x / xRadius, direction.y / yRadius);
        Vector2 clampedNormalized = Vector2.ClampMagnitude(normalized, 1f);
        Vector2 clampedOffset = new Vector2(clampedNormalized.x * xRadius, clampedNormalized.y * yRadius);

        pupil.position = (Vector2)eyeCenter.position + clampedOffset;
    }

    private void OnDrawGizmos()
    {
        if(eyeCenter == null) return;

        Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(eyeCenter.position, radius);

        Vector3 prevPoint = eyeCenter.position + new Vector3(xRadius, 0f, 0f);
        for(int i = 0; i <= 32; i++)
        {
            float angle = (i / 32f) * Mathf.PI * 2f;
            Vector3 point = eyeCenter.position + new Vector3(Mathf.Cos(angle) * xRadius, Mathf.Sin(angle) * yRadius, 0f);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }
    }
}
