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

    public CubertNeeds[] cubertNeeds;
}
