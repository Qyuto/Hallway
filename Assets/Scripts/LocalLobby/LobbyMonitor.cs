using DG.Tweening;
using UnityEngine;

namespace LocalLobby
{
    public class LobbyMonitor : MonoBehaviour
    {
        private MoveCamera _moveCamera;
        private void Awake()
        {
            _moveCamera = GetComponent<MoveCamera>();
        }

        public void ToggleMonitor(bool status, TweenCallback onArrive)
        {
            if (status)
                _moveCamera.MoveToTarget().onComplete += onArrive;
            else
                _moveCamera.MoveToMainPosition().onComplete += onArrive;
        }
    }
}