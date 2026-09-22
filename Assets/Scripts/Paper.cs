using UnityEngine;

public class Paper : MonoBehaviour
{
    [SerializeField] private GameObject paperObj;
    [SerializeField] private AudioClip paperSFX;

    void Awake()
    {
        paperObj.SetActive(false);
    }

    void OnMouseDown()
    {
        if(!paperObj.activeSelf)
        {
            paperObj.SetActive(true);
            SoundManager.Singleton?.PlaySFX(paperSFX);
        }
    }

    public void ClosePaper()
    {
        DaycareScreen.Singleton?.PaperRead(this);
        paperObj.SetActive(false);
    }
}
