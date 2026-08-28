using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CubertSpots { Center, LitterBox, Bed, FrontDesk }

public class CubertRoomSpot : MonoBehaviour
{
    [SerializeField] private CubertSpots cubertSpot;

    private Cubert cubert;
    private float timer = 0f;

    public Cubert CubertInSpot => cubert;
    public float Timer => timer;
    
    void Update()
    {
        if(cubert != null)
        {
            timer += Time.deltaTime;
        }
    }

    void OnMouseDown()
    {
        if(HoldCubert.Singleton.HoldingCubert)
        {
            PutCubertInSpot(HoldCubert.Singleton.HeldCubert.gameObject);
        }
    }

    public void PutCubertInSpot(GameObject cubertObj)
    {
        HoldCubert holdCubert = HoldCubert.Singleton;

        if(cubertSpot is not CubertSpots.FrontDesk)
        {
            if(ScreenManager.Singleton.CurrentScreen.gameObject.name == cubertObj.name)
            {
                switch(cubertSpot)
                {
                    case CubertSpots.Center:
                        {
                            holdCubert.DropCubertInRoom();
                            break;
                        }
                    case CubertSpots.LitterBox:
                        {
                            holdCubert.DropCubertInLitterBox();
                            break;
                        }
                    case CubertSpots.Bed:
                        {
                            holdCubert.DropCubertInBed();
                            break;
                        }
                }
            }
        }
        else
        {
            holdCubert.DropCubertInDaycare();
        }

        cubert = cubertObj.GetComponent<Cubert>();
    }

    public void TakeCubertFromSpot()
    {
        cubert = null;
        timer = 0f;
    }
}
