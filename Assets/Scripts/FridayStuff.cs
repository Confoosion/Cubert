using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    [SerializeField] private NPC_Tim timT;
    public NPC_Tim TimT => timT;
    [SerializeField] private NPC_Tim timO;
    public NPC_Tim TimO => timO;

    [SerializeField] private GameObject dimBG;

    public void EnterTims()
    {
        // timT.SetData(timT_SO, Purpose.PickUp);
        timT.NPCEnter();
        // timO.SetData(timO_SO, Purpose.PickUp);
        timO.NPCEnter();
    }

    public void HubertEnding()
    {
        StartCoroutine(HubertEnd());
    }

    IEnumerator HubertEnd()
    {
        yield return new WaitForSeconds(1);
        
        yield return DarkenScreen();
        
    }

    IEnumerator DarkenScreen()
    {
        dimBG.SetActive(true);
        Image image = dimBG.GetComponent<Image>();
        Color currentColor = image.color;
        float currentAlpha = currentColor.a;

        while(currentAlpha < 1f)
        {
            currentAlpha += Time.deltaTime * 1.5f;
            currentColor.a = currentAlpha;

            image.color = currentColor;
            yield return null;
        }

        yield return null;
    }
}
