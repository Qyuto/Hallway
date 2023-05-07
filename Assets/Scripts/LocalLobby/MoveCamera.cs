using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace LocalLobby
{
    public class MoveCamera : MonoBehaviour
    {
        [SerializeField] private float duration;
        [SerializeField] private Vector3 nextPosition;
        [SerializeField] private Vector3 nextEulerRotation;

        public Vector3 CurrentPosition => _targetCamera.transform.position;

        private Camera _targetCamera;
        private Vector3 _prevPosition;
        private Vector3 _prevEulerRotation;
        private Transform _cameraParent;

        public TweenerCore<Vector3, Vector3, VectorOptions> MoveToTarget()
        {
            if (_targetCamera == null)
            {
                _targetCamera = Camera.main;
                _cameraParent = _targetCamera.transform.parent;
            }

            _prevPosition = _targetCamera.transform.localPosition;
            _prevEulerRotation = _targetCamera.transform.localRotation.eulerAngles;
            _targetCamera.transform.parent = null;
            var doMove = _targetCamera.transform.DOMove(transform.position + nextPosition, duration);
            _targetCamera.transform.DORotate(nextEulerRotation, duration);

            return doMove;
        }

        public TweenerCore<Vector3, Vector3, VectorOptions> MoveToMainPosition()
        {
            _targetCamera.transform.parent = _cameraParent;
            var doMove = _targetCamera.transform.DOLocalMove(_prevPosition, duration);
            _targetCamera.transform.DOLocalRotate(_prevEulerRotation, duration);

            return doMove;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + nextPosition, 0.05f);
        }
    }
}