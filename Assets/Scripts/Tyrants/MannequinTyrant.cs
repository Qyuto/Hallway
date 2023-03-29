using Mirror;
using Network;
using Unity.VisualScripting;
using UnityEngine;

namespace Tyrants
{
    [RequireComponent(typeof(TyrantMoveLogic))]
    public class MannequinTyrant : BaseTyrant
    {
        [SerializeField, SyncVar] private bool _isBeingObserved;

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
            if (_isBeingObserved) return;
            if (!_moveLogic.FindClosestIdentityInRange(out NetworkIdentity networkIdentity, findRadius, playerMask, QueryTriggerInteraction.Ignore)) return;
            if (!networkIdentity.TryGetComponent(out LocalPlayer target)) return;
            _moveCoroutine ??= StartCoroutine(_moveLogic.MoveToTarget(target.transform, 5, () => target.CmdKillPlayer()));
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
            _isBeingObserved = status;
            if (!status) return;
            if (_moveCoroutine == null) return;
            StopCoroutine(_moveCoroutine);
            StartCoroutine(_moveLogic.MoveToTarget(_moveLogic.transform, 1, null));
            _moveCoroutine = null;
        }
        
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, findRadius);
        }
    }
}