using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubertScreen : MonoBehaviour
{
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private GameObject foodParticle;
    [SerializeField] private Transform foodTransform;

    [SerializeField] private Transform cubert;

    private float feedCooldown = 1f;
    private float timer = 0f;

    void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
    }

    public void FeedCubert()
    {
        if(timer > 0f) return;
        if(FoodManager.Singleton.FoodAmount < 1) return;

        timer = feedCooldown;
        FoodManager.Singleton.UseFood(1);

        StartCoroutine(Feeding());
    }

    IEnumerator Feeding()
    {
        GameObject food = Instantiate(foodPrefab, foodTransform.position, Quaternion.identity);

        food.GetComponent<Rigidbody2D>().simulated = false;
        food.GetComponent<CircleCollider2D>().enabled = false;

        Transform target = cubert;
        
        Vector3 startPos = food.transform.position;
        Vector3 startScale = food.transform.localScale;

        float duration = 0.35f;
        float _time = 0f;
        
        while(_time < duration)
        {
            _time += Time.deltaTime;
            float t = Mathf.Clamp01(_time / duration);
            float eased = 1f - Mathf.Pow(1f - t, 2f);

            food.transform.position = Vector3.Lerp(startPos, target.position, eased);
            food.transform.localScale = Vector3.Lerp(startScale, startScale * 0.3f, eased);
            
            yield return null;
        }

        food.transform.position = target.position;
        food.transform.localScale = startScale * 0.3f;

        Destroy(food.gameObject);
        Instantiate(foodParticle, new Vector3(cubert.position.x, cubert.position.y, -2f), Quaternion.identity);
    }
}
