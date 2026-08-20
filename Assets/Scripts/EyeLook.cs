using UnityEngine;

public class EyeLook : MonoBehaviour
{
    [SerializeField] private Transform pupil;
    [SerializeField] private Transform eyeCenter;
    [SerializeField] private float radius = 0.2f;

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
        Vector2 clampedOffset = Vector2.ClampMagnitude(direction, radius);

        pupil.position = (Vector2)eyeCenter.position + clampedOffset;
    }

    private void OnDrawGizmos()
    {
        if(eyeCenter == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(eyeCenter.position, radius);
    }
}
