using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class RoomDestroyer : MonoBehaviour // Server side only
{
    private static Queue<GameObject> rooms = new Queue<GameObject>();
    private const int MaxRoomCount = 5;
    
    public static void AddRoom(GameObject nRoom)
    {
        rooms.Enqueue(nRoom);
        if(rooms.Count > MaxRoomCount)
            DestroyRoom();
    }

    private static void DestroyRoom()
    {
        NetworkServer.Destroy(rooms.Dequeue());
    }
}
