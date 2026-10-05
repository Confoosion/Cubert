using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class MubertStuff : MonoBehaviour
{
    [SerializeField] Color normalColor;
    [SerializeField] Color hidingColor;
    [SerializeField] private SpriteRenderer bloodRenderer;
    [SerializeField] private AudioClip biteSound;
    
    [Space]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] giggles;
    [SerializeField] private AudioClip crazyLaugh;

    private bool isStuck = false;
    public bool IsStuck => isStuck;

    public bool HideInCubertsRoom(CubertScreen mubertRoom)
    {
        Transform cubertRoom = ScreenManager.Singleton.Screens.Find(room => room.name == "Cubert");
        if(ScreenManager.Singleton.CurrentScreen == cubertRoom) return false;

        mubertRoom.DisplayFeedButton(false);

        transform.parent = cubertRoom;
        transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        transform.localPosition = new Vector2(3.9f, -0.75f);

        GetComponent<SpriteRenderer>().color = hidingColor;

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in renderers)
        {
            sr.sortingLayerName = "Background";
        }
        
        GetComponent<Cubert>().SetLookTarget(cubertRoom.GetComponent<CubertScreen>().CubertTransform);
        
        return true;  
    }

    public void GoToDaycare(CubertScreen mubertRoom)
    {
        Transform daycareScreen = ScreenManager.Singleton.Screens[0];
        transform.parent = daycareScreen;
        transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
        transform.localPosition = new Vector2(0f, 0f);

        GetComponent<Cubert>().SetLookTarget(null);
        isStuck = true;

    }

    public void EatRandomCubert()
    {
        CubertScreen eatenCubert = ScreenManager.Singleton.GetRandomAliveCubert();
        eatenCubert.Kill();
        GlobalEvents.Singleton.AddMubertKill(eatenCubert);
        
        SoundManager.Singleton?.PlaySFX(biteSound);
        ShowBlood(true);
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
        GlobalEvents.Singleton.MubertDisappeared(true);
    }

    public void Appear()
    {
        gameObject.SetActive(true);
        CubertScreen mubertRoom = transform.parent.GetComponent<CubertScreen>();

        mubertRoom.UpdateNeedStatus();
    }

    public void ChangeToNormalColor()
    {
        GetComponent<SpriteRenderer>().color = normalColor;
    }

    public void ShowBlood(bool show)
    {
        bloodRenderer.enabled = show;

        if(show)
        {
            CubertScreen mubertScreen = ScreenManager.Singleton.GetCubertScreen("Mubert");
            mubertScreen.ForceAddNeed(mubertScreen.BasicNeeds[1]);
        }
    }

    public void GoCrazy()
    {
        StartCoroutine(Crazy());
    }

    IEnumerator Crazy()
    {
        AudioClip mubertGiggle;
        while(ScreenManager.Singleton.CurrentScreen.name != "DAYCARE")
        {
            mubertGiggle = giggles[UnityEngine.Random.Range(0, giggles.Length)];

            audioSource.PlayOneShot(mubertGiggle);

            yield return new WaitForSeconds(mubertGiggle.length);
        }

        audioSource.PlayOneShot(crazyLaugh);
        yield return new WaitForSeconds(5.4f);

        Debug.Log("Attack");
    }
}
