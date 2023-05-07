using Mirror;
using UnityEngine;

namespace Tyrants
{
    [RequireComponent(typeof(TyrantMoveLogic))]
    public class TyrantPhantom : BaseTyrant
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip clip;

        private TyrantMoveLogic _moveLogic;

        private void Awake()
        {
            _moveLogic = GetComponent<TyrantMoveLogic>();
            OnAttackingChanged.AddListener(PlayClip);
        }

        private void Update()
        {
            if (!isServer || IsAttacking) return;
            if (!_moveLogic.FindClosestIdentityInRange(out NetworkIdentity target, findRadius, targetMask, QueryTriggerInteraction.Ignore)) return;
            CmdSetSelectedUser(target);
            StartCoroutine(_moveLogic.MoveToTarget(target.transform, 1f, CmdHideTyrant));
            IsAttacking = true;
        }

        [Command(requiresAuthority = false)]
        private void CmdHideTyrant()
        {
            NetworkServer.Destroy(gameObject);
        }

        private void PlayClip(bool newValue)
        {
            Debug.Log("Player Sound");
            audioSource.transform.parent = null;
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(audioSource.gameObject, 5f);
        }
    }
}