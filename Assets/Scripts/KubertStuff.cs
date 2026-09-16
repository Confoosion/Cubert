using UnityEngine;
using System.Collections;

public class KubertStuff : MonoBehaviour
{
    [SerializeField] private AudioClip kubertFart;
    [SerializeField] Sprite deadSprite;

    public void DeathFart(CubertScreen cubertScreen)
    {
        StartCoroutine(KubertDeath(cubertScreen));
    }

    IEnumerator KubertDeath(CubertScreen cubertScreen)
    {
        SoundManager.Singleton?.PlaySFX(kubertFart);
        yield return new WaitForSeconds(kubertFart.length);

        SpriteRenderer[] renderers = cubertScreen._Cubert.gameObject.GetComponentsInChildren<SpriteRenderer>();
        for(int i = 0; i < renderers.Length; i++)
        {
            if(i != 0)
                renderers[i].enabled = false;
            else
                renderers[i].sprite = deadSprite;
        }

        cubertScreen.ForceAddNeed(cubertScreen.SpecialNeeds[2], false);
    }
}
