using UnityEngine;

public class FridayStuff : MonoBehaviour
{
    [SerializeField] private GameObject lubertObj;
    public GameObject LubertObj => lubertObj;
    [SerializeField] private GameObject kubertObj;
    public GameObject KubertObj => kubertObj;
    [SerializeField] private GameObject hubertObj;
    public GameObject HubertObj => hubertObj;

    [SerializeField] private NPCSO christinaNPC;
    public NPCSO ChristinaNPC => christinaNPC;

    [SerializeField] private NPC timT;
    [SerializeField] private NPCSO timT_SO;
    public NPC TimT => timT;
    [SerializeField] private NPC timO;
    [SerializeField] private NPCSO timO_SO;
    public NPC TimO => timO;

    public void EnterTims()
    {
        // timT.SetData(timT_SO, Purpose.PickUp);
        timT.NPCEnter();
        // timO.SetData(timO_SO, Purpose.PickUp);
        timO.NPCEnter();
    }
}
