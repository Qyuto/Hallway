using Mirror;
using UnityEngine;

public class HideMesh : NetworkBehaviour
{
    [SerializeField] private GameObject[] meshs;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
        {
            foreach (var mesh in meshs)
                mesh.SetActive(false);
        }
    }
}
