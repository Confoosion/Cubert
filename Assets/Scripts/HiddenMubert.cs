using UnityEngine;

public class HiddenMubert : MonoBehaviour
{
    [SerializeField] private Transform hidingInRoom;

    public void SetRoom(Transform room)
    {
        hidingInRoom = room;
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
                break;
            }
        }

        if(mubert != null)
        {
            HoldCubert.Singleton.GrabCubert(mubert);
            GameObject.Find("TuesdayStuff").GetComponent<TuesdayStuff>().DisplayHiddenMuberts(false);
        }
    }

    void Update()
    {
        if(hidingInRoom == null)
            return;

        if(ScreenManager.Singleton.CurrentScreen != hidingInRoom)
        {
            // GameObject.SetActive(false);
        }

    }
}
