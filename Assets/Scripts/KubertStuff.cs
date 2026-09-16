using UnityEngine;
using System.Collections;

public class KubertStuff : MonoBehaviour
{
    [SerializeField] private AudioClip kubertFart;
    [SerializeField] Sprite deadSprite;

    public void DeathFart(GameObject cubertObj)
    {
        StartCoroutine(KubertDeath(cubertObj));
    }

    IEnumerator KubertDeath(GameObject cubertObj)
    {
        SoundManager.Singleton?.PlaySFX(kubertFart);
        yield return new WaitForSeconds(kubertFart.length);

        SpriteRenderer[] renderers = cubertObj.GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < renderers.Length; i++)
        {
            if(i != 0)
                renderers[i].enabled = false;
            else
                renderers[i].sprite = deadSprite;
        }
    }
}
