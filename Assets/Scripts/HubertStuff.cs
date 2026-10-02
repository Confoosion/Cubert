using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class HubertStuff : MonoBehaviour
{    
    [SerializeField] private int timesVisited = 0;
    private Transform roomIn;
    private bool stuck = false;
    public bool IsStuck => stuck;

    [SerializeField] private GameObject deadHubert;

    private CubertScreen hubertsRoom;
    private Coroutine killRoutine;

    public bool AttemptLeave(CubertScreen hubertRoom)
    {
        hubertsRoom = hubertRoom;

        if(roomIn != null) return false;

        CubertScreen cubertRoom = ScreenManager.Singleton.GetCubertScreen("Tubert");
        if(cubertRoom == null || cubertRoom.Dead) cubertRoom = ScreenManager.Singleton.GetCubertScreen("Oubert");

        if(ScreenManager.Singleton.CurrentScreen == cubertRoom.transform && TimeManager.Singleton.GetDay() != "Thursday" || cubertRoom.Dead)
            return false;

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
            transform.parent = cubertRoom.transform;
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

                if(timesVisited >= 3)
                {
                    GlobalEvents.Singleton.KillTubert();
                }
            }
            else if(!TimeManager.Singleton.IsNight && !TimeManager.Singleton.IsMorning)
            {
                roomIn = cubertRoom.transform;
                killRoutine = StartCoroutine(KillDelay());
            }

            CubertScreen cbrt = cubertRoom.GetComponent<CubertScreen>();
            GetComponent<Cubert>().SetLookTarget(cbrt.CubertTransform);
            cbrt.ForceAddScared();
        }

        return true;
    }

    IEnumerator KillDelay()
    {
        yield return new WaitForSeconds(9f);
        while(ScreenManager.Singleton.CurrentScreen == roomIn)
        {
            yield return new WaitForSeconds(2.7f);
        }
        
        if(roomIn.name == "Tubert")
        {
            Debug.Log("Tubert killed by Hubert!");
            hubertsRoom.PlaceCubert(hubertsRoom._Cubert.gameObject);
            ResetKill();
            GlobalEvents.Singleton.KillTubert();
            hubertsRoom._Cubert.UnlockEyes();
        }
        else if(roomIn.name == "Oubert")
        {
            Debug.Log("Oubert killed by Hubert!");
            hubertsRoom.PlaceCubert(hubertsRoom._Cubert.gameObject);
            ResetKill();
            GlobalEvents.Singleton.KillOubert();
            hubertsRoom._Cubert.UnlockEyes();
        }
    }

    public void ResetKill()
    {
        roomIn = null;
        if(killRoutine != null)
            StopCoroutine(killRoutine);
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
            KillHubert();
        }
    }

    public void GetStuck()
    {
        stuck = true;
    }

    public void KillHubert()
    {
        gameObject.SetActive(false);
        GameObject dead = Instantiate(deadHubert, transform.position, Quaternion.identity);
        dead.transform.localScale = transform.localScale;

        ScreenManager.Singleton.GetCubertScreen("Hubert").Kill();
        GlobalEvents.Singleton.KillHubert();
    }
}
