using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScreenMap : MonoBehaviour
{
    [SerializeField] private GameObject roomObj;
    [SerializeField] private List<Image> map = new List<Image>();

    [SerializeField] private Color chillRoomColor;
    [SerializeField] private Color SelectedRoomColor;
    [SerializeField] private Color NeedRoomColor;

    [SerializeField] private int selectedRoom;


    void Start()
    {
        SetSelectedRoom(0);
    }

    public void AddScreenToMap()
    {
        GameObject room = Instantiate(roomObj, transform);
    
        map.Insert(1, room.GetComponent<Image>());
    }

    public void RemoveScreenFromMap(Image roomToRemove)
    {
        Image room = map.Find(r => r.GetComponent<Image>() == roomToRemove);
        if(room != null)
        {
            map.Remove(room);
            Destroy(room.gameObject);
        }
    }
    
    public void SetSelectedRoom(int roomNumber)
    {
        map[selectedRoom].color = chillRoomColor;

        selectedRoom = roomNumber;
        map[roomNumber].color = SelectedRoomColor;
    }
}
