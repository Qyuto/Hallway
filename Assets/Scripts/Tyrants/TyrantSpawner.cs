using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class TyrantSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] tyrants;
    [SerializeField] private Transform spawnPosition;

    [Range(1, 100), SerializeField] private int spawnPercent;

    private GameObject _instanceTyrant;
    
    public override void OnStartServer()
    {
        base.OnStartServer();
        CmdSpawnTyrant();
    }

    [Command(requiresAuthority = false)]
    private void CmdSpawnTyrant()
    {
        if (Random.Range(0, 100) <= spawnPercent)
        {
            _instanceTyrant = Instantiate(tyrants[Random.Range(0, tyrants.Length)], spawnPosition.position, spawnPosition.rotation);
            NetworkServer.Spawn(_instanceTyrant);
        }
    }

    private void OnDestroy()
    {
        if(isServer && _instanceTyrant != null)
            NetworkServer.Destroy(_instanceTyrant);
    }
}