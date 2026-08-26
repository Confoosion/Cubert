using UnityEngine;

public class Cubert : MonoBehaviour
{
    [SerializeField] private CubertScreen.Need[] needs;
    [SerializeField] private int feedAmount = 3;
    [SerializeField] private int hitAmount = 3;

    public CubertScreen.Need[] Needs => needs;
    public int FeedAmount => feedAmount;
    public int HitAmount => hitAmount;

    private int timesHitWithBall;
    public int TimesHitWithBall => timesHitWithBall;

    private HoldCubert holdCubert;
    private CubertScreen homeScreen;


    void Start()
    {
        holdCubert = HoldCubert.Singleton;
    }

    void OnMouseDown()
    {
        if(!holdCubert.HoldingCubert)
            holdCubert.GrabCubert(this);
    }

    public void SetHome(CubertScreen screen)
    {
        homeScreen = screen;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BouncyBall ball = collision.gameObject.GetComponent<BouncyBall>();
        if(ball != null && homeScreen.CurrentNeed?.need.needName == "Bored")
        {
            if(!ball.IsDragging)
            {
                Debug.Log("Hit with ball");
                timesHitWithBall++;

                if(timesHitWithBall >= hitAmount)
                {
                    homeScreen.SatisfyNeed();
                }
            }
        }
    }
}
