using UnityEngine;

[CreateAssetMenu(fileName = "NewNeedsList", menuName = "Cubert/NeedsList")]
public class CubertNeedsSO : ScriptableObject
{
    [System.Serializable]
    public class CubertNeeds
    {
        public Day day;
        public NeedsSO[] needs;
    }

    [System.Serializable]
    public class CubertHabits
    {
        public Day day;
        public float timeNeeded;
        public Habit[] habits;
    }

    public CubertNeeds[] cubertNeeds;
    public CubertHabits[] cubertHabits;
}
