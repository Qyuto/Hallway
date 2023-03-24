using Mirror;
using Network;
using UnityEngine;

namespace Tyrants
{
    [RequireComponent(typeof(TyrantMoveLogic))]
    public class MannequinTyrant : BaseTyrant
    {
        [SyncVar] private bool _playerIsLooking;

        [SerializeField] private LayerMask playerMask;
        [SerializeField] private float findRadius;

        private TyrantMoveLogic _moveLogic;
        private Coroutine _moveCoroutine;

        private void Awake()
        {
            _moveLogic = GetComponent<TyrantMoveLogic>();
        }

        private void FixedUpdate() // Server side only
        {
            if (!isServer) return;
            if (_playerIsLooking) return;

            if (!_moveLogic.FindTarget(out NetworkIdentity networkIdentity, findRadius, playerMask, QueryTriggerInteraction.Ignore)) return;
            if (!networkIdentity.TryGetComponent(out LocalPlayer target)) return;
            _moveCoroutine = StartCoroutine(_moveLogic.MoveToTarget(target.transform, 10, (() => target.CmdKillPlayer())));
        }

        public override void OnSelect(NetworkIdentity networkIdentity)
        {
            base.OnSelect(networkIdentity);
            CmdSetLookingStatus(true);
        }

        public override void OnDeSelect(NetworkIdentity networkIdentity)
        {
            base.OnDeSelect(networkIdentity);
            CmdSetLookingStatus(false);
        }

        [Command(requiresAuthority = false)]
        private void CmdSetLookingStatus(bool status)
        {
            _playerIsLooking = status;
            if (status)
            {
                if (_moveCoroutine != null)
                    StopCoroutine(_moveCoroutine);
                _moveCoroutine = StartCoroutine(_moveLogic.MoveToTarget(_moveLogic.transform, 0, null));
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, findRadius);
        }
    }
}