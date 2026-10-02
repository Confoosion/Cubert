using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private EndingDialogue endDialogue;

    void OnMouseDown()
    {
        dialogue?.DisplayNextSentence();
        endDialogue?.DisplayNextSentence();
    }
}
