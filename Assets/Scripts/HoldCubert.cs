using UnityEngine;

public class HoldCubert : MonoBehaviour
{
    public static HoldCubert Singleton;

    [SerializeField] private Cubert heldCubert;
    [SerializeField] private GameObject heldBall;

    public Cubert HeldCubert => heldCubert;
    public bool HoldingCubert => heldCubert != null;
    public GameObject HeldBall => heldBall;
    public bool HoldingBall => heldBall != null;

    [SerializeField] private Vector3 holdPosition;
    [SerializeField] private Vector3 holdScale;

    void Awake()
    {
        if (Singleton == null) Singleton = this;
    }

    public void GrabCubert(Cubert cubert)
    {
        if(heldCubert != null || heldBall != null) return;

        if(ScreenManager.Singleton.CurrentScreen == DaycareScreen.Singleton.transform
           && (DaycareScreen.Singleton.Npc.Interacted && DaycareScreen.Singleton.CurrentNPC != null)) return;

        heldCubert = cubert;
        cubert.GetComponent<BoxCollider2D>().enabled = false;
        cubert.transform.SetParent(transform);
        cubert.transform.localPosition = holdPosition;
        cubert.transform.localScale = holdScale;

        cubert.DisplaySleepParticles(false);

        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>()?._Cubert == cubert)
        {
            ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>()?.DisplayFeedButton(false);
        }

        SetSortingLayer("PickedUp");
    }

    private void SetSortingLayer(string sortingName)
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in renderers)
        {
            sr.sortingLayerName = sortingName;
        }
    }

    public void DropCubertInRoom()
    {
        SetSortingLayer("Default");
        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>().PlaceCubert(heldCubert.gameObject))
        {
            heldCubert = null;
        }
        else
        {
            SetSortingLayer("PickedUp");
        }
    }

    public void DropCubertInLitterBox()
    {
        SetSortingLayer("Default");
        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>().PlaceCubertInLitterBox(heldCubert.gameObject))
        {
            heldCubert = null;
        }
        else
        {
            SetSortingLayer("PickedUp");
        }
    }

    public void DropCubertInBed()
    {
        SetSortingLayer("Default");
        if(ScreenManager.Singleton.CurrentScreen.GetComponent<CubertScreen>().PlaceCubertInBed(heldCubert.gameObject))
        {
            heldCubert = null;
        }
        else
        {
            SetSortingLayer("PickedUp");
        }
    }

    public void DropCubertInDaycare()
    {
        SetSortingLayer("Default");
        if(DaycareScreen.Singleton.PlaceCubertOnFrontDesk(heldCubert.gameObject))
        {
            heldCubert = null;
        }
        else
        {
            SetSortingLayer("PickedUp");
        }
    }

    public void GrabBall(GameObject ball)
    {
        if(heldCubert != null || heldBall != null) return;

        heldBall = ball;
        Rigidbody2D ballRb = ball.GetComponent<Rigidbody2D>();
        ballRb.linearVelocity = Vector2.zero;
        ballRb.bodyType = RigidbodyType2D.Kinematic;

        ball.transform.SetParent(transform);
        ball.transform.localPosition = holdPosition;
        ball.transform.localScale = holdScale;
    }

    public void DropBall()
    {
        if(heldBall == null) return;
        
        heldBall.transform.SetParent(ScreenManager.Singleton.CurrentScreen);
        heldBall = null;
    }
}
