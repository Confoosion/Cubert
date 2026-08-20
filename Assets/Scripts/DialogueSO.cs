using UnityEngine;
using System.Collections;

public enum DialogueType { Intro, PickUp, Cubert }

[CreateAssetMenu(fileName = "NewDialogue", menuName = "NPC/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public DialogueType dialogueType;

    [TextArea(3, 10)]
    public string[] sentences;
}