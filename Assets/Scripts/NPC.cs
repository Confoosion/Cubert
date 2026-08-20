using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCSO _data;
    public NPCSO Data => _data;
    [SerializeField] private Sprite npcSprite;
    public Sprite GetSprite() { return npcSprite; }

    public bool interacted;

    public void StartIntroDialogue(Dialogue dialogue)
    {
        dialogue.StartDialogue(_data.npcName, _data.introDialogue);
    }

    public void StartPickUpDialogue(Dialogue dialogue)
    {
        dialogue.StartDialogue(_data.npcName, _data.pickUpDialogue);
    }

    public void StartCubertDialogue(Dialogue dialogue)
    {
        dialogue.StartDialogue(_data.npcName, _data.cubertDialogue);
    }

    void OnMouseDown()
    {
        if(!interacted)
            interacted = true;
    }

    public void SetData(NPCSO data)
    {
        _data = data;
    }

    public void EnterDaycare()
    {
        
    }

    public void ExitDaycare()
    {
        
    }

    // IEnumerator NPCAnimation(bool enter)
    // {
    //     float t = 0f;
    //     while(t < scaleInDuration)
    //     {
    //         t += Time.deltaTime;
    //         float normalized = Mathf.Clamp01(t / scaleInDuration);
    //         float eval = scaleCurve.Evaluate(normalized);
    //         transform.localScale = Vector3.LerpUnclamped(Vector3.zero, targetScale, eval);
    //         yield return null;
    //     }
    //     transform.localScale = targetScale;

    //     float 
    // }
}
