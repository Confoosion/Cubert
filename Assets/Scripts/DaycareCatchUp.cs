using UnityEngine;

public class DaycareCatchUp : MonoBehaviour
{
    [SerializeField] private GameObject[] cubertsOvernight;

    void Start()
    {
        if(cubertsOvernight.Length > 0)
        {
            foreach(GameObject cubert in cubertsOvernight)
            {
                if(cubert.name == "Hubert" && GlobalEvents.Singleton.HubertDead) continue;
                ScreenManager.Singleton.SetCubert(cubert);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
