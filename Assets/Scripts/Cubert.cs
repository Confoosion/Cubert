using UnityEngine;

public class Cubert : MonoBehaviour
{
    [SerializeField] private CubertNeedsSO cubertNeeds;
    [SerializeField] private int feedAmount = 3;
    [SerializeField] private int hitAmount = 3;
    [SerializeField] private int showerAmount = 20;

    [SerializeField] private GameObject sleepParticles;

    public CubertNeedsSO CubertNeeds => cubertNeeds;
    public int FeedAmount => feedAmount;
    public int HitAmount => hitAmount;
    public int ShowerAmount => showerAmount;

    private int timesHitWithBall;
    public int TimesHitWithBall => timesHitWithBall;

    private int timesHitWithShower;
    public int TimesHitWithShower => timesHitWithShower;

    private HoldCubert holdCubert;
    private CubertScreen homeScreen;

    private int needsCompleted = 0;


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
        if(ball != null && homeScreen?.CurrentNeed?.needName == "Bored")
        {
            if(!ball.IsDragging)
            {
                Debug.Log("Hit with ball");
                timesHitWithBall++;

                if(timesHitWithBall >= hitAmount)
                {
                    homeScreen.SatisfyNeed();
                    timesHitWithBall = 0;
                }
            }
        }
    }

    void OnParticleCollision(GameObject particle)
    {
        if(homeScreen.CurrentNeed?.needName == "Dirty")
        {
            timesHitWithShower++;

            if(timesHitWithShower >= showerAmount)
            {
                homeScreen.SatisfyNeed();
                timesHitWithShower = 0;
            }
        }
    }

    public void DisplaySleepParticles(bool show)
    {
        if(show)
        {
            GameObject particles = Instantiate(sleepParticles, this.transform);
            particles.transform.localPosition = new Vector3(0f, 0.5f, -0.25f);
        }
        else
        {
            ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
            if(particles != null)
            {
                particles.Stop();
                Destroy(particles.gameObject);
            }
        }
    }

    public void HideCubert()
    {
        gameObject.SetActive(false);
    }

    public void ShowCubert()
    {
        gameObject.SetActive(true);
    }
}
