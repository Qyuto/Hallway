using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Tyrants
{
    public abstract class BaseTyrant : NetworkBehaviour
    {
        [SerializeField] protected LayerMask targetMask;
        [SerializeField] protected float findRadius = 3f;

        [SyncVar(hook = nameof(OnAttackingStatusChanged)), SerializeField] private bool isAttacking; // Mb it's not good idea
        [SyncVar, SerializeField] protected NetworkIdentity lastSelected;

        protected bool IsAttacking
        {
            get => isAttacking;
            set
            {
                if (isServer)
                    isAttacking = value;
            }
        }
        protected UnityEvent<bool> OnAttackingChanged = new UnityEvent<bool>();

        public virtual void OnSelect(NetworkIdentity networkIdentity)
        {
            if (networkIdentity != null)
                CmdSetSelectedUser(networkIdentity);
        }

        public virtual void OnDeSelect(NetworkIdentity networkIdentity)
        {
        }

        [Command(requiresAuthority = false)]
        protected void CmdSetSelectedUser(NetworkIdentity user)
        {
            lastSelected = user;
        }

        private void OnAttackingStatusChanged(bool oldStatus, bool newStatus)
        {
            OnAttackingChanged?.Invoke(newStatus);
        }
    }
}