using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static GlobalEvents Singleton;

    void Awake()
    {
        if(Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] private bool oubertUncomfortable = false;
    public bool OubertUncomfortable => oubertUncomfortable;
    [SerializeField] private bool annaAngry = false;
    public bool AnnaAngry => annaAngry;
    [SerializeField] private bool hubertDead = false;
    public bool HubertDead => hubertDead;
    [SerializeField] private bool mubertGone = false;
    public bool MubertGone => mubertGone;
    [SerializeField] private bool tubertDead = false;
    public bool TubertDead => tubertDead;
    [SerializeField] private bool fubertMakeupRuined = false;
    public bool FubertMakeupRuined => fubertMakeupRuined;
    [SerializeField] private bool oubertDead = false;
    public bool OubertDead => oubertDead;
    // [SerializeField] private bool jerryDead = false;

    public void GotOubertUncomfortable()
    {
        oubertUncomfortable = true;
    }

    public void GotAnnaAngry()
    {
        annaAngry = true;
    }

    public void KillHubert()
    {
        hubertDead = true;
    }

    public void MubertDisappeared(bool gone)
    {
        mubertGone = gone;
    }

    public void KillTubert()
    {
        if(tubertDead) return;
        tubertDead = true;
        if(TimeManager.Singleton.IsNight)
            DaycareScreen.Singleton.NPCLeave();
    }

    public void RuinFubertMakeup()
    {
        fubertMakeupRuined = true;
    }

    public void KillOubert()
    {
        if(oubertDead) return;
        oubertDead = true;
    }
}
