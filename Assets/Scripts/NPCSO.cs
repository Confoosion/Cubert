using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "NPC/NPC")]
public class NPCSO : ScriptableObject
{
    public string npcName;

    public Color skinColor;
    public Color shirtColor;

    public GameObject cubert;
    
    public bool pickupSameDay = false;
    public int pickupDay = 0;

    public DialogueSO introDialogue;
    public DialogueSO pickUpDialogue;
    public DialogueSO cubertDialogue;

    
}
