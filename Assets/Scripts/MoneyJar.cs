using UnityEngine;

public class MoneyJar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    
    [SerializeField] private Sprite[] jarSprites;

    public void UpdateJar(int amount)
    {
        sr.sprite = jarSprites[amount];
    }
}
