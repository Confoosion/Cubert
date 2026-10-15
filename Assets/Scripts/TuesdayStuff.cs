using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TuesdayStuff : MonoBehaviour
{
    [SerializeField] private GameObject mubertPrefab;
    [SerializeField] private Vector2 mubertHiddenPosition = new Vector2(-4f, 0.83f);
    [SerializeField] private Vector3 mubertHiddenScale = new Vector3(1.75f, 1.75f, 1.75f);
    [SerializeField] private List<GameObject> hiddenMuberts = new List<GameObject>();

    public void SpawnHiddenMuberts()
    {
        List<Transform> cubertScreens = ScreenManager.Singleton.Screens;

        foreach(Transform room in cubertScreens)
        {
            if(room.name == "KITCHEN" || room.name == "DAYCARE" ) continue;

            if(room.name == "Cubert")
            {
                GameObject mubert = Instantiate(mubertPrefab, room);
                mubert.GetComponent<HiddenMubert>().SetRoom(room);
                mubert.transform.localScale = mubertHiddenScale;
                mubert.transform.localPosition = mubertHiddenPosition;
                hiddenMuberts.Add(mubert);
            }

            if(room.name == "Mubert")
            {
                CubertScreen mubertRoom = room.GetComponent<CubertScreen>();
                mubertRoom.TurnLightsOff();
                mubertRoom.ForceAddNeed(mubertRoom.SpecialNeeds[0], false);
                mubertRoom._Cubert.HideCubert();
            }
        }
    }

    public void DisplayHiddenMuberts(bool show)
    {
        foreach(GameObject mubert in hiddenMuberts)
        {
            if(mubert == null) continue;
            mubert.SetActive(show);
        }
    }
}
