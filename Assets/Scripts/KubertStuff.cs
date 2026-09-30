using UnityEngine;
using System.Collections;

public class KubertStuff : MonoBehaviour
{
    [SerializeField] private AudioClip kubertFart;
    [SerializeField] private SpriteRenderer face;
    [SerializeField] private SpriteRenderer deadFace;
    [SerializeField] private SpriteRenderer[] eyes;
    private bool stuck = false;
    public bool IsStuck => stuck;

    public void DeathFart(CubertScreen cubertScreen)
    {
        stuck = true;
        StartCoroutine(KubertDeath(cubertScreen));
    }

    IEnumerator KubertDeath(CubertScreen cubertScreen)
    {
        SoundManager.Singleton?.PlaySFX(kubertFart);
        yield return new WaitForSeconds(kubertFart.length);

        foreach(SpriteRenderer eye in eyes)
        {
            eye.enabled = false;
        }
        face.enabled = false;
        deadFace.enabled = true;

        cubertScreen.ForceAddNeed(cubertScreen.SpecialNeeds[2], false);
    }
}
