using UnityEngine;
using System.Collections.Generic;

public class JubertStuff : MonoBehaviour
{
    public void GoSomewhere(CubertScreen jubertRoom)
    {
        jubertRoom.DisplayFeedButton(false);

        CubertScreen newRoom = ScreenManager.Singleton.GetRandomCubertScreen();
        if(newRoom == null) return;

        transform.parent = newRoom.transform;
        
        List<Transform> spots = new List<Transform>() {newRoom.Nest, newRoom.LitterBox, newRoom.Bed};
        spots.Remove(newRoom.CurrentSpot);

        switch(spots[Random.Range(0, spots.Count)].name)
        {
            case "Nest":
                {
                    newRoom.ForceCubertInNest(gameObject);
                    break;
                }
            case "LitterBox":
                {
                    newRoom.ForceCubertInLitterBox(gameObject);
                    break;
                }
            case "Bed":
                {
                    newRoom.ForceCubertInBed(gameObject);
                    break;
                }
        }

    }
}
