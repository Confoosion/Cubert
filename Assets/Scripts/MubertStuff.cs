using UnityEngine;
using System;
using System.Linq;

public class MubertStuff : MonoBehaviour
{
    [SerializeField] Color normalColor;
    [SerializeField] Color hidingColor;
    
    public void HideInCubertsRoom(CubertScreen mubertRoom)
    {
        Transform cubertRoom = ScreenManager.Singleton.Screens.Find(room => room.name == "Cubert");
        if(ScreenManager.Singleton.CurrentScreen == cubertRoom) return;

        mubertRoom.DisplayFeedButton(false);

        transform.parent = cubertRoom;
        transform.localScale = new Vector3(2f, 2f, 2f);
        transform.localPosition = new Vector2(3.25f, 0.3f);

        GetComponent<SpriteRenderer>().color = hidingColor;

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in renderers)
        {
            sr.sortingLayerName = "Background";
        }
        
        GetComponent<Cubert>().SetLookTarget(cubertRoom.GetComponent<CubertScreen>().CubertTransform);
    }

    public void ChangeToNormalColor()
    {
        GetComponent<SpriteRenderer>().color = normalColor;
    }
}
