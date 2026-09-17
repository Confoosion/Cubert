using UnityEngine;
using System;
using System.Linq;

public class HubertStuff : MonoBehaviour
{    
    [SerializeField] private int timesVisited = 0;

    public bool AttemptLeave(CubertScreen hubertRoom)
    {
        Transform cubertRoom = ScreenManager.Singleton.Screens.Find(room => room.name == "Tubert");
        if(cubertRoom == null) cubertRoom = ScreenManager.Singleton.Screens.Find(room => room.name == "Oubert");

        if(ScreenManager.Singleton.CurrentScreen == cubertRoom && TimeManager.Singleton.GetDay() != "Thursday") return false;

        hubertRoom.DisplayFeedButton(false);

        if(FoodManager.Singleton.HasFoodInKitchen)
        {
            Transform kitchen = ScreenManager.Singleton.Screens.Find(room => room.name == "KITCHEN");
            if(ScreenManager.Singleton.CurrentScreen == kitchen) return false;

            transform.parent = kitchen;
            transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
            transform.localPosition = new Vector2(0f, -3.25f);

            FoodManager.Singleton.DestroyAllFoodInKitchen(); 
        }
        else
        {
            transform.parent = cubertRoom;
            transform.localScale = new Vector3(4f, 4f, 4f);
            transform.localPosition = new Vector2(-4f, -2.75f);

            // SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
            // foreach(SpriteRenderer sr in renderers)
            // {
            //     sr.sortingLayerName = "Background";
            // }
            
            if(TimeManager.Singleton.GetDay() == "Thursday" && TimeManager.Singleton.IsNight)
            {
                timesVisited++;

                if(timesVisited >= 4)
                {
                    GlobalEvents.Singleton.KillTubert();
                }
            }

            CubertScreen cbrt = cubertRoom.GetComponent<CubertScreen>();
            GetComponent<Cubert>().SetLookTarget(cbrt.CubertTransform);
            cbrt.ForceAddScared();
        }

        return true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Food>() != null && !GlobalEvents.Singleton.HubertDead)
        {
            SoundManager.Singleton?.PlayEatSFX();
            collision.gameObject.GetComponent<Food>().Eat();
        }
        if(collision.gameObject.GetComponent<Knife>())
        {
            GlobalEvents.Singleton.KillHubert();
        }
    }
}
