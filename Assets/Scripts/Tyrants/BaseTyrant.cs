using Mirror;
using UnityEngine;

namespace Tyrants
{
    public abstract class BaseTyrant : NetworkBehaviour
    {
        [SyncVar, SerializeField] protected NetworkIdentity lastSelected;

        public virtual void OnSelect(NetworkIdentity networkIdentity)
        {
            if(networkIdentity != null)
                CmdSetSelectedUser(networkIdentity);
        }

        public virtual void OnDeSelect(NetworkIdentity networkIdentity)
        {
        }

        [Command (requiresAuthority = false)]
        protected void CmdSetSelectedUser(NetworkIdentity user)
        {
            lastSelected = user;
        }
    }
}