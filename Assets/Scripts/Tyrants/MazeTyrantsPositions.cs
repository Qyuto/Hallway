using UnityEngine;

namespace Tyrants
{
    public class MazeTyrantsPositions : MonoBehaviour
    {
        [SerializeField] private TyrantSpawner spawner;
        [SerializeField] private Transform[] teleportPositions;

        private void Awake()
        {
            spawner.OnTyrantSpawned.AddListener(SetTeleportPositions);
        }

        private void SetTeleportPositions(GameObject tyrant)
        {
            if (!tyrant.TryGetComponent(out MazeMannequinTyrant mannequinTyrant)) return;
            if (mannequinTyrant == null) return;
            mannequinTyrant.SetTeleportPosition(teleportPositions);
        }
    }
}