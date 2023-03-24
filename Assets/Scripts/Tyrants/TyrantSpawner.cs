using Mirror;
using UnityEngine;

public class TyrantSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject[] tyrants;
    [SerializeField] private Transform spawnPosition;

    [Range(1, 100), SerializeField] private int spawnPercent;

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
            GameObject tyrant = Instantiate(tyrants[Random.Range(0, tyrants.Length)], spawnPosition.position, spawnPosition.rotation);
            NetworkServer.Spawn(tyrant);
        }
    }
}