using UnityEngine;

public class WednesdayStuff : MonoBehaviour
{
    public void HubertLeave()
    {
        Transform hubertRoom = ScreenManager.Singleton.Screens.Find(room => room.name == "Hubert");
        CubertScreen hubertScreen = hubertRoom.GetComponent<CubertScreen>(); 
        
        hubertScreen.PerformHabit(Habit.HubertLeave);
        if(hubertScreen.CubertTransform.parent.name == "Oubert")
        {
            GlobalEvents.Singleton.GotOubertUncomfortable();
        }
    }
}
