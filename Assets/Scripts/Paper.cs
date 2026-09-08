using UnityEngine;

public class Paper : MonoBehaviour
{
    [SerializeField] private GameObject paperObj;

    void Awake()
    {
        ClosePaper();
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
        paperObj.SetActive(false);
    }
}
