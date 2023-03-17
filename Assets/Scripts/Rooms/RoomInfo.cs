using UnityEngine;

namespace Rooms
{
    [CreateAssetMenu(fileName = "RoomInfo", menuName = "Room/Room", order = 0)]
    public class RoomInfo : ScriptableObject
    {
        public string RoomId;
        [SerializeField] private GameObject[] RoomsPrefab;

        public GameObject GetRandomRoom()
        {
            return RoomsPrefab[Random.Range(0, RoomsPrefab.Length)];
        }
    }
}