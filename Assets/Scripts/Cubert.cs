using UnityEngine;

public class Cubert : MonoBehaviour
{
    void OnMouseDown()
    {
        HoldCubert.Singleton.GrabCubert(this);
    }
}
