using Network;
using Tyrants;
using UnityEngine;

namespace Player
{
    public class FindTyrant : MonoBehaviour
    {
        [SerializeField] private LayerMask tyrantMask;
        [SerializeField] private float findDistance = 4f;

        private Transform _viewTransform;
        private BaseTyrant _lastTyrant;

        private void Awake()
        {
            _viewTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (Physics.Raycast(_viewTransform.position, _viewTransform.forward, out RaycastHit hitInfo, findDistance, tyrantMask, QueryTriggerInteraction.Ignore))
            {
                _lastTyrant = hitInfo.collider.GetComponentInParent<BaseTyrant>();
                if (_lastTyrant == null) return;
                _lastTyrant.OnSelect(LocalPlayer.Player.netIdentity);
            }
            else if (_lastTyrant != null)
            {
                _lastTyrant.OnDeSelect(LocalPlayer.Player.netIdentity);
                _lastTyrant = null;
            }
        }
    }
}