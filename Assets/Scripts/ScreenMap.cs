using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScreenMap : MonoBehaviour
{
    [SerializeField] private GameObject roomObj;
    [SerializeField] private List<Image> map = new List<Image>();
    [SerializeField] private List<Image> needMap = new List<Image>();

    [SerializeField] private Color chillRoomColor;
    [SerializeField] private Color selectedRoomColor;
    [SerializeField] private Color needRoomColor;

    [SerializeField] private int selectedRoom;


    void Start()
    {
        SetSelectedRoom(0);
    }

    public void AddScreenToMap()
    {
        GameObject room = Instantiate(roomObj, transform);
    
        // map.Insert(1, room.GetComponent<Image>());
        map.Add(room.GetComponent<Image>());
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
        Debug.Log("Selected " + roomNumber);
        int oldRoom = selectedRoom;
        selectedRoom = roomNumber;
        
        SetChillRoom(oldRoom);
        map[roomNumber].color = selectedRoomColor;
    }

    public void SetNeedRoom(int roomNumber)
    {
        Debug.Log("Need " + roomNumber);

        if(selectedRoom != roomNumber)
            map[roomNumber].color = needRoomColor;

        if(!needMap.Contains(map[roomNumber]))
            needMap.Add(map[roomNumber]);
    }
    
    public void RemoveNeedRoom(int roomNumber)
    {
        if(needMap.Contains(map[roomNumber]))
        {
            needMap.Remove(map[roomNumber]);
        }
    }

    public void SetChillRoom(int roomNumber)
    {
        if(needMap.Contains(map[roomNumber]))
        {
            SetNeedRoom(roomNumber);
            return;
        }

        Debug.Log("Chill " + roomNumber);
        map[roomNumber].color = chillRoomColor;
    }
}
