using UnityEngine;
using UnityEngine.InputSystem;

namespace LocalLobby
{
    public class PlayerFindMonitor : MonoBehaviour
    {
        [SerializeField] private InputActionReference findMonitor;
        [SerializeField] private InputActionReference resumePlayerView;

        [SerializeField] private MouseRotation rotation;
        [SerializeField] private PlayerMove playerMove;

        [SerializeField] private LayerMask monitorMask;
        [SerializeField] private float rayDistance;

        private Transform _viewTransform;
        private LobbyMonitor _lobbyMonitor;
        private bool _isWatchingOnMonitor;

        private void Awake()
        {
            _viewTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            findMonitor.action.performed += FindMonitor;
            findMonitor.action.Enable();

            resumePlayerView.action.performed += ResumePlayerView;
            resumePlayerView.action.Enable();
        }

        private void DisableMotionComponents()
        {
            rotation.enabled = playerMove.enabled = false;
        }

        private void EnableMotionComponents()
        {
            rotation.enabled = playerMove.enabled = true;
        }

        private void ResumePlayerView(InputAction.CallbackContext obj)
        {
            if (_lobbyMonitor == null) return;
            _lobbyMonitor.ToggleMonitor(false, EnableMotionComponents);
            _lobbyMonitor = null;
            _isWatchingOnMonitor = false;
        }

        private void FindMonitor(InputAction.CallbackContext obj)
        {
            if (!_isWatchingOnMonitor && Physics.Raycast(_viewTransform.position, _viewTransform.forward, out RaycastHit hit, rayDistance, monitorMask))
            {
                _lobbyMonitor = hit.collider.GetComponentInParent<LobbyMonitor>();
                if (_lobbyMonitor == null) return;
                _lobbyMonitor.ToggleMonitor(true, DisableMotionComponents);
                _isWatchingOnMonitor = true;
            }
        }
    }
}