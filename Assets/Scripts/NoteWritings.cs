using UnityEngine;

[CreateAssetMenu(fileName = "Note", menuName = "Note")]
public class NoteWritings : ScriptableObject
{
    [TextArea(3, 10)]

    public string writing;
}
