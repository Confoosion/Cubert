using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [SerializeField] private AudioClip NPCEnterSound;

    [Space]
    [SerializeField] private SpriteRenderer[] normalDaycareParts;
    [SerializeField] private SpriteRenderer[] bloodyDaycareParts;
    [SerializeField] private SpriteRenderer daycareBG;
    [SerializeField] private SpriteRenderer daycareDesk;
    [SerializeField] private Sprite normalDaycareBG;
    [SerializeField] private Sprite normalDesk;
    [SerializeField] private Sprite bloodyDaycareBG;
    [SerializeField] private Sprite bloodyDesk;

    public void EnterTims()
    {
        timT.NPCEnter();
        timO.NPCEnter();
        
        ScreenManager.Singleton.SetNPCScreen(true);
        SoundManager.Singleton?.PlaySFX(NPCEnterSound);
        DaycareScreen.Singleton.DisableCubertDropOff(false);
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

        SceneManager.LoadScene("HubertEnding");
        yield return null;
    }

    public void MubertEnding()
    {
        StartCoroutine(MubertEnd());
    }

    IEnumerator MubertEnd()
    {
        bool isScary = true;
        SwitchDaycareBG(isScary);
        
        yield return null;
    }

    private void SwitchDaycareBG(bool scary)
    {
        foreach(SpriteRenderer part in normalDaycareParts)
        {
            part.enabled = !scary;
        }
        foreach(SpriteRenderer part in bloodyDaycareParts)
        {
            part.enabled = scary;
        }

        daycareBG.sprite = scary ? bloodyDaycareBG : normalDaycareBG;
        daycareDesk.sprite = scary ? bloodyDesk : normalDesk;
    }
}
