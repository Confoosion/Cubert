using UnityEngine;

public class WednesdayStuff : MonoBehaviour
{
    [SerializeField] private bool isAnnaHappy = true;
    public bool IsAnnaHappy => isAnnaHappy;

    public void HubertLeave()
    {
        Transform hubertRoom = ScreenManager.Singleton.Screens.Find(room => room.name == "Hubert");
        hubertRoom.GetComponent<CubertScreen>().PerformHabit(Habit.HubertLeave);
    }

    public void MakeAnnaMad()
    {
        isAnnaHappy = false;
    }
}
