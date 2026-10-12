using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private EndingDialogue endDialogue;

    void OnMouseDown()
    {
        if(dialogue != null)
        {
            dialogue.DisplayNextSentence();
            return;
        }

        if(endDialogue != null)
        {
            endDialogue.DisplayNextSentence();
            return;
        }
    }
}
