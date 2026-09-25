using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private float followSpeed = 20f;
    private bool isHolding = false;

    private Rigidbody2D rb;
    private Camera cam;

    private Vector2 homePosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        homePosition = transform.localPosition;
    }


    private void OnMouseDown()
    {
        isHolding = true;
        Cursor.visible = false;
    }

    private void OnMouseUp()
    {
        isHolding = false;
        Cursor.visible = true;

        transform.localPosition = homePosition;
    }

    void FixedUpdate()
    {
        if(!isHolding) return;

        Vector2 targetPos = GetMouseWorldPosition();
        Vector2 newPos = Vector2.Lerp(rb.position, targetPos, followSpeed * Time.fixedDeltaTime);

        rb.MovePosition(newPos);
    }

    private Vector2 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(screenPos);
    }
}
