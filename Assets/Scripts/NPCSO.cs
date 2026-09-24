using UnityEngine;

[CreateAssetMenu(fileName = "NewNPC", menuName = "NPC/NPC")]
public class NPCSO : ScriptableObject
{
    public string npcName;

    public Sprite npcFrontSprite;
    public Sprite npcBackSprite;

    public GameObject cubert;

    [System.Serializable]
    public class NPCDialogue
    {
        public Day day;
        public DialogueSO introDialogue;
        public DialogueSO pickUpDialogue;
        public DialogueSO cubertDialogue;
    }

    public NPCDialogue[] npcDialogue;
    public DialogueSO wrongCubertDialogue;
    public NPCDialogue[] branchDialogue;
    
    [Range(0.5f, 2f)]
    public float voicePitch = 1f;
    public int voiceFrequency = 3;
}
