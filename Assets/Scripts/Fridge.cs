using UnityEngine;

public class Fridge : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private float minForce = 2f;
    [SerializeField] private float maxForce = 4f;
    [SerializeField] private float minAngle = -60f;
    [SerializeField] private float maxAngle = -120f;

    private float foodCooldown = 0.5f;
    private float timer;

    void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void SpawnFood()
    {
        if(timer > 0f) return;

        timer = foodCooldown;
        GameObject food = Instantiate(foodPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = food.GetComponent<Rigidbody2D>();

        float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        float forceAmount = Random.Range(minForce, maxForce);

        rb.AddForce(direction * forceAmount, ForceMode2D.Impulse);   
    }
}
