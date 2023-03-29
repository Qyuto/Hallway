using Mirror;
using Network;
using UnityEngine;

namespace Rooms
{
    public class RoomSpawner : NetworkBehaviour
    {
        [SerializeField] private RoomInfo nextRoom;
        [SerializeField] private Transform nextPosition;

        [SyncVar] private bool _isSpawned;

        [Command(requiresAuthority = false)]
        public void CmdSpawnRoom(NetworkIdentity calledSpawn)
        {
            if (_isSpawned) return;
            GameObject prefab;
            GameObject newRoom;
            if (calledSpawn.TryGetComponent(out LocalPlayer player) && player.PlayerDistance >= nextRoom.DistanceToFinalRoom)
            {
                prefab = nextRoom.GetFinalRoom();
                newRoom = Instantiate(prefab, nextPosition.position, nextPosition.rotation);
            }
            else
            {
                prefab = nextRoom.GetRandomRoom();
                newRoom = Instantiate(prefab, nextPosition.position, nextPosition.rotation);
            }
            NetworkServer.Spawn(newRoom);
            MapBrightnessReducer reducer = newRoom.GetComponent<MapBrightnessReducer>();
            reducer.CmdChangeBrightness(calledSpawn.GetComponent<LocalPlayer>().PlayerDistance);
            RoomDestroyer.AddRoom(newRoom);
            _isSpawned = true;
        }
    }
}