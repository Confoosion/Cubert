using UnityEngine;

public class Paper : MonoBehaviour
{
    [SerializeField] private GameObject paperObj;

    void Awake()
    {
        paperObj.SetActive(false);
    }

    void OnMouseDown()
    {
        if(!paperObj.activeSelf)
        {
            paperObj.SetActive(true);
        }
    }

    public void ClosePaper()
    {
        DaycareScreen.Singleton?.PaperRead(this);
        paperObj.SetActive(false);
    }
}
