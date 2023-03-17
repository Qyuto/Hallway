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
        
        [Command (requiresAuthority = false)]
        public void CmdSpawnRoom(NetworkIdentity calledSpawn)
        {
            if(_isSpawned) return;
            GameObject prefab = nextRoom.GetRandomRoom();
            GameObject newRoom = Instantiate(prefab, nextPosition.position, prefab.transform.rotation);
            NetworkServer.Spawn(newRoom);

            MapBrightnessReducer reducer = newRoom.GetComponent<MapBrightnessReducer>();
            reducer.CmdChangeBrightness(calledSpawn.GetComponent<LocalPlayer>().PlayerDistance);
            RoomDestroyer.AddRoom(newRoom);
            _isSpawned = true;
        }
    }
}