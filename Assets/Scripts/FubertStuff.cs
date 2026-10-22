using UnityEngine;

public class FubertStuff : MonoBehaviour
{
    [SerializeField] private Sprite bloodyLipsFace;
    [SerializeField] private Sprite noMakeupFace;
    [SerializeField] private SpriteRenderer faceRenderer; 

    void Start()
    {
        if(TimeManager.Singleton.GetDay() == "Friday")
        {
            SwitchToBloodyLips();
            CubertScreen fubertScreen = transform.parent.GetComponent<CubertScreen>();
            fubertScreen.ForceAddNeed(fubertScreen.BasicNeeds[1]);
        }
    }

    public void SwitchToBloodyLips()
    {
        faceRenderer.sprite = bloodyLipsFace;
    }

    public void SwitchToNoMakeup()
    {
        GlobalEvents.Singleton.RuinFubertMakeup();
        faceRenderer.sprite = noMakeupFace;
    }
}
