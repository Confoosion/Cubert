using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThursdayStuff : MonoBehaviour
{
    [SerializeField] private NPCSO christinaNPC;
    public NPCSO ChristinaNPC => christinaNPC;

    private bool interactedWithTim = false;
    public bool InteractedWithTim => interactedWithTim;
    public void InteractWithTim() => interactedWithTim = true;

    [Space]
    [SerializeField] TextMeshProUGUI clipboardText;
    [SerializeField] private NoteWritings cubertDeadNote;
    [SerializeField] private NoteWritings cubertAliveNote;

    void Start()
    {
        SetClipboardNote();
    }

    private void SetClipboardNote()
    {
        if(GlobalEvents.Singleton.CubertAte)
        {
            clipboardText.SetText(cubertDeadNote.writing);
        }
        else
        {
            clipboardText.SetText(cubertAliveNote.writing);
        }
    }
}
