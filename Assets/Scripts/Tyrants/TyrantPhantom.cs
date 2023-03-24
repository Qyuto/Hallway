using Mirror;
using UnityEngine;

namespace Tyrants
{
    [RequireComponent(typeof(TyrantMoveLogic))]
    public class TyrantPhantom : BaseTyrant
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip clip;
        [SerializeField] private float findRadius;
        [SerializeField] private LayerMask playerMask;

        private TyrantMoveLogic _moveLogic;
        [SyncVar(hook = nameof(PlayClip))] private bool _isAttacking;

        private void Awake()
        {
            _moveLogic = GetComponent<TyrantMoveLogic>();
        }

        private void Update()
        {
            if (!isServer || _isAttacking) return;
            if (!_moveLogic.FindTarget(out NetworkIdentity target, findRadius, playerMask, QueryTriggerInteraction.Ignore)) return;
            CmdSetSelectedUser(target);
            StartCoroutine(_moveLogic.MoveToTarget(target.transform, 1f, CmdHideTyrant));
            _isAttacking = true;
        }

        [Command(requiresAuthority = false)]
        private void CmdHideTyrant()
        {
            NetworkServer.Destroy(gameObject);
        }

        private void PlayClip(bool oldValue, bool newValue)
        {
            audioSource.transform.parent = null;
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(audioSource.gameObject,5f);
        }
    }
}