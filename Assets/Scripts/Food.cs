using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Food : MonoBehaviour
{
    private bool isCollected = false;

    private void OnMouseDown()
    {
        if(!isCollected)
        {
            isCollected = true;

            StartCoroutine(Collecting());
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButton(0) && !isCollected)
        {
            isCollected = true;

            StartCoroutine(Collecting());            
        }
    }

    IEnumerator Collecting()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<CircleCollider2D>().enabled = false;

        Transform target = FoodManager.Singleton.FoodHolder;
        
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;

        float duration = 0.35f;
        float _time = 0f;
        
        while(_time < duration)
        {
            _time += Time.deltaTime;
            float t = Mathf.Clamp01(_time / duration);
            float eased = 1f - Mathf.Pow(1f - t, 2f);

            transform.position = Vector3.Lerp(startPos, target.position, eased);
            transform.localScale = Vector3.Lerp(startScale, startScale * 0.3f, eased);
            
            yield return null;
        }

        transform.position = target.position;
        transform.localScale = startScale * 0.3f;

        FoodManager.Singleton.AddFood(1);
        Destroy(gameObject);
    }
}
