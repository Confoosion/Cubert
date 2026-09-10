using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TuesdayStuff : MonoBehaviour
{
    [SerializeField] private GameObject mubertPrefab;
    [SerializeField] private Vector2 mubertHiddenPosition = new Vector2(-22f, 0.83f);
    [SerializeField] private List<GameObject> hiddenMuberts = new List<GameObject>();

    public void SpawnHiddenMuberts()
    {
        List<Transform> cubertScreens = ScreenManager.Singleton.Screens;
        cubertScreens.RemoveAt(0);
        cubertScreens.RemoveAt(cubertScreens.Count - 1);

        foreach(Transform room in cubertScreens)
        {
            GameObject mubert = Instantiate(mubertPrefab, room);
            mubert.GetComponent<HiddenMubert>().SetRoom(room);
            mubert.transform.localPosition = mubertHiddenPosition;
        }
    }
}
