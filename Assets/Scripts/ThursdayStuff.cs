using UnityEngine;

public class ThursdayStuff : MonoBehaviour
{
    [SerializeField] private NPCSO christinaNPC;
    public NPCSO ChristinaNPC => christinaNPC;

    private bool interactedWithTim = false;
    public bool InteractedWithTim => interactedWithTim;
    public void InteractWithTim() => interactedWithTim = true;
}
