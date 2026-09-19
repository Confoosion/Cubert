using UnityEngine;
using System.Collections.Generic;

public class DaycareCatchUp : MonoBehaviour
{
    // [SerializeField] private GameObject[] cubertsOvernight;
    [SerializeField] private List<GameObject> cubertsOvernight = new List<GameObject>();

    void Start()
    {
        if(TimeManager.Singleton.GetDay() == "Friday")
        {
            FridayStuff fridayStuff = GameObject.Find("FridayStuff").GetComponent<FridayStuff>();
            if(!GlobalEvents.Singleton.AnnaAngry)
            {
                cubertsOvernight.Insert(1, fridayStuff.LubertObj);
            }
            else
            {
                cubertsOvernight.Insert(1, fridayStuff.KubertObj);    
            }

            if(!GlobalEvents.Singleton.HubertDead)
            {
                cubertsOvernight.Add(fridayStuff.HubertObj);
            }
        }

        if(cubertsOvernight.Count > 0)
        {
            foreach(GameObject cubert in cubertsOvernight)
            {
                if(cubert.name == "Hubert" && GlobalEvents.Singleton.HubertDead) continue;
                ScreenManager.Singleton.SetCubert(cubert);
            }
        }
    }
}
