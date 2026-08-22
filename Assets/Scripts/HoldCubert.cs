using UnityEngine;

public class HoldCubert : MonoBehaviour
{
    public static HoldCubert Singleton;

    [SerializeField] private Cubert heldCubert;
    public Cubert HeldCubert => heldCubert;
    public bool HoldingCubert => heldCubert != null;

    [SerializeField] private Vector3 holdPosition;
    [SerializeField] private Vector3 holdScale;

    void Awake()
    {
        if (Singleton == null) Singleton = this;
    }

    public void GrabCubert(Cubert cubert)
    {
        if(heldCubert != null) return;

        heldCubert = cubert;
        cubert.transform.SetParent(transform);
        cubert.transform.localPosition = holdPosition;
        cubert.transform.localScale = holdScale;
    }

    public void DropCubertInRoom()
    {
        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>().PlaceCubert(heldCubert.gameObject))
            heldCubert = null;
    }

    public void DropCubertInLitterBox()
    {
        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>().PlaceCubertInLitterBox(heldCubert.gameObject))
            heldCubert = null;
    }

    public void DropCubertInBed()
    {
        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>().PlaceCubertInBed(heldCubert.gameObject))
            heldCubert = null;
    }

    public void DropCubertInBathtub()
    {
        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>().PlaceCubertInBathtub(heldCubert.gameObject))
            heldCubert = null;
    }

    public void DropCubertInPlayArea()
    {
        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>().PlaceCubertInPlayArea(heldCubert.gameObject))
            heldCubert = null;
    }

    public void DropCubertInDaycare()
    {
        
    }
}
