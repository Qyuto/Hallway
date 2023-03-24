using System;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class LocalPlayer : NetworkBehaviour
    {
        [SyncVar, SerializeField] private float playerDistance;
        [SyncVar, SerializeField] private bool isHide;
        [SyncVar(hook = nameof(OnDeathStatusChange))] private bool isDeath;
        
        public float PlayerDistance => playerDistance;
        public bool IsHide => isHide;
        public bool IsDeath => isDeath;

        
        
        public UnityEvent OnPlayerDeath;
        public static LocalPlayer Player;

        public override void OnStartLocalPlayer()
        {
            Player = this;
            base.OnStartLocalPlayer();
        }

        [Command(requiresAuthority = false)]
        public void CmdUpdatePlayerDistance(float newDistance)
        {
            if (newDistance < 0) newDistance = 0;
            playerDistance = newDistance;
        }

        [Command(requiresAuthority = false)]
        public void CmdChangeHideStatus(bool newStatus)
        {
            isHide = newStatus;
        }

        [Command (requiresAuthority = false)]
        public void CmdKillPlayer()
        {
            isDeath = true;
        }

        private void OnDeathStatusChange(bool oldStatus, bool newStatus)
        {
            if(newStatus)
                OnPlayerDeath?.Invoke();
        }
    }
}