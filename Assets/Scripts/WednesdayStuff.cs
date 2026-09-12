using UnityEngine;

public class WednesdayStuff : MonoBehaviour
{
    public void HubertLeave()
    {
        Transform hubertRoom = ScreenManager.Singleton.Screens.Find(room => room.name == "Hubert");
        hubertRoom.GetComponent<CubertScreen>().PerformHabit(Habit.HubertLeave);
    }
}
