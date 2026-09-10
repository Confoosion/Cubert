using UnityEngine;

public class HiddenMubert : MonoBehaviour
{
    [SerializeField] private Transform hidingInRoom;

    public void SetRoom(Transform room)
    {
        hidingInRoom = room;
    }

    void Update()
    {
        if(hidingInRoom == null)
            return;

        if(ScreenManager.Singleton.CurrentScreen != hidingInRoom)
        {
            // GameObject.SetActive(false);
        }

    }
}
