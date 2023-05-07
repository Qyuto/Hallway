using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tyrants
{
    [RequireComponent(typeof(TyrantMoveLogic))]
    public class MazeMannequinTyrant : BaseTyrant
    {
        [SerializeField] private float tyrantSpeedMultiplier = 1.2f;

        private TyrantMoveLogic _moveLogic;
        private Transform[] _teleportPositions;
        private NetworkIdentity _lastFindIdentity;
        private Coroutine _moveCoroutine;

        private void Awake()
        {
            _moveLogic = GetComponent<TyrantMoveLogic>();
        }

        private void Update()
        {
            if (!isServer) return;

            if (_moveLogic.FindClosestIdentityInRange(out NetworkIdentity target, findRadius, targetMask, QueryTriggerInteraction.Ignore))
            {
                // if (_lastFindIdentity == null)
                // {
                //     _lastFindIdentity = target;
                //     _moveCoroutine = StartCoroutine(_moveLogic.MoveToTarget(target.transform, 0f, (() => Debug.Log("Maze tyrant find player"))));
                // }
                // if (_lastFindIdentity == target) return;

                if (_moveCoroutine != null)
                {
                    StopCoroutine(_moveCoroutine);
                    _moveCoroutine = null;
                }

                _moveCoroutine = StartCoroutine(_moveLogic.MoveToTarget(target.transform, 0f, (() => Debug.Log("Maze tyrant find player"))));
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdChangeTyrantPosition()
        {
            if (_teleportPositions == null) return;
            if (_moveLogic.isMoving && _moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }

            Vector3 nextPosition = _teleportPositions[Random.Range(0, _teleportPositions.Length)].position;
            transform.position = nextPosition;
            _moveLogic.Agent.speed *= tyrantSpeedMultiplier;
        }

        public void SetTeleportPosition(Transform[] positions)
        {
            _teleportPositions = positions;
        }

        public override void OnSelect(NetworkIdentity networkIdentity)
        {
            base.OnSelect(networkIdentity);
            CmdChangeTyrantPosition();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, findRadius);
        }
    }
}