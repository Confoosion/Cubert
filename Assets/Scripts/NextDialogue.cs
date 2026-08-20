using UnityEngine;

public class NextDialogue : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    void OnMouseDown()
    {
        dialogue.DisplayNextSentence();
    }
}
