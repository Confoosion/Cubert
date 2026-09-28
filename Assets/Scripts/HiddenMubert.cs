using UnityEngine;
using System.Collections;

public class HiddenMubert : MonoBehaviour
{
    [SerializeField] private Transform hidingInRoom;
    [SerializeField] private EyeLook[] eyes;
    
    private float fadeInDuration = 1f;

    public void SetRoom(Transform room)
    {
        hidingInRoom = room;

        foreach(EyeLook eye in eyes)
        {
            eye.LookAtTarget(room.GetComponent<CubertScreen>().CubertTransform);
        }

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        float[] targetAlphas = new float[renderers.Length];

        for(int i = 0; i < renderers.Length; i++)
        {
            targetAlphas[i] = renderers[i].color.a;
            SetAlpha(renderers[i], 0f);
        }

        // Loop
        float elapsed = 0f;
        while(elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeInDuration);

            for(int i = 0; i < renderers.Length; i++)
            {
                SetAlpha(renderers[i], Mathf.Lerp(0f, targetAlphas[i], t));
            }    

            yield return null;
        }

        for(int i = 0; i < renderers.Length; i++)
        {
            SetAlpha(renderers[i], targetAlphas[i]);
        }
    }

    private void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }

    void OnMouseDown()
    {
        Cubert mubert = null;

        foreach(var screen in ScreenManager.Singleton.Screens)
        {
            CubertScreen cbrtScreen = screen.GetComponent<CubertScreen>();
            if(cbrtScreen == null) continue;

            Cubert cubert = cbrtScreen._Cubert;

            if(cubert.gameObject.name == "Mubert")
            {
                mubert = cubert;
                cbrtScreen.EnableColliders();
                break;
            }
        }

        if(mubert != null)
        {
            HoldCubert.Singleton.GrabCubert(mubert);
            GameObject.Find("TuesdayStuff").GetComponent<TuesdayStuff>().DisplayHiddenMuberts(false);
        }
    }
}
