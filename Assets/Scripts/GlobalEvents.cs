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

    // [SerializeField] private bool oubertDead = false;
    // [SerializeField] private bool tubertDead = false;
    // [SerializeField] private bool jerryDead = false;

    public void GotOubertUncomfortable()
    {
        oubertUncomfortable = true;
    }

    public void GotAnnaAngry()
    {
        annaAngry = true;
    }
}
