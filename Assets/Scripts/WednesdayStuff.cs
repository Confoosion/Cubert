using UnityEngine;

public class WednesdayStuff : MonoBehaviour
{
    public void HubertLeave()
    {
        CubertScreen hubertScreen = ScreenManager.Singleton.GetCubertScreen("Hubert");
        
        hubertScreen.PerformHabit(Habit.HubertLeave);
        if(hubertScreen.CubertTransform.parent.name == "Oubert")
        {
            GlobalEvents.Singleton.GotOubertUncomfortable();
        }
    }

    public void KillCubert()
    {
        CubertScreen cubertScreen = ScreenManager.Singleton.GetCubertScreen("Cubert");
        cubertScreen.Kill();
        cubertScreen.TurnLightsOff();

        // Make the room look bloody too.

        CubertScreen mubertScreen = ScreenManager.Singleton.GetCubertScreen("Mubert");
        mubertScreen.PerformHabit(Habit.MubertLeave);

        // Also make Mubert bloody
        mubertScreen._Cubert.GetComponent<MubertStuff>().ShowBlood(true);
    }
}
