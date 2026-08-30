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

    public void DropCubertInDaycare()
    {
        
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
