using Mirror;
using UnityEngine;

public class PilotInitialization : NetworkBehaviour
{
    [SerializeField] private GameObject LocalPilot;
    [SerializeField] private GameObject RemotePilot;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
            Instantiate(LocalPilot, transform.position, Quaternion.identity, transform);
        else
            Instantiate(RemotePilot, transform.position, Quaternion.identity, transform);
    }
}