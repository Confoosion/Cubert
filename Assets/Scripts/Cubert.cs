using UnityEngine;

public class Cubert : MonoBehaviour
{
    [SerializeField] private CubertScreen.Need[] needs;
    [SerializeField] private int feedAmount = 3;

    public CubertScreen.Need[] Needs => needs;
    public int FeedAmount => feedAmount;

    private HoldCubert holdCubert;


    void Start()
    {
        holdCubert = HoldCubert.Singleton;
    }

    void OnMouseDown()
    {
        if(!holdCubert.HoldingCubert)
            holdCubert.GrabCubert(this);
        // else if(holdCubert.HeldCubert == this)
        // {
        //     // Debug.Log("Clicked held Cubert");
        //     // Debug.Log(ScreenManager.Singleton.CurrentScreen.gameObject.name);
        //     // Debug.Log(gameObject.name);
        //     if(ScreenManager.Singleton.CurrentScreen.gameObject.name == gameObject.name)
        //     {
        //         holdCubert.DropCubertInRoom();
        //     }
        //     else if(ScreenManager.Singleton.CurrentScreen.gameObject.name == "DAYCARE")
        //     {
        //         holdCubert.DropCubertInDaycare();
        //     }
        // }
    }
}
