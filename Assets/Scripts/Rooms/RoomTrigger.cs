using Mirror;
using UnityEngine;

namespace Rooms
{
    public class RoomTrigger : MonoBehaviour
    {
        [SerializeField] private RoomSpawner roomSpawner;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                roomSpawner.CmdSpawnRoom(other.GetComponent<NetworkIdentity>());
        }
    }
}