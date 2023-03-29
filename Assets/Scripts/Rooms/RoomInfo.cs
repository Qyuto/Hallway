using UnityEngine;

namespace Rooms
{
    [CreateAssetMenu(fileName = "RoomInfo", menuName = "Room/Room", order = 0)]
    public class RoomInfo : ScriptableObject
    {
        public string roomId;
        [SerializeField] private GameObject[] RoomsPrefab;
        [SerializeField] private float distanceToFinalRoom = 3000;
        [SerializeField] private GameObject finalRoomPrefab;

        public float DistanceToFinalRoom => distanceToFinalRoom;
        
        public GameObject GetRandomRoom()
        {
            return RoomsPrefab[Random.Range(0, RoomsPrefab.Length)];
        }

        public GameObject GetFinalRoom()
        {
            return finalRoomPrefab;
        }
    }
}