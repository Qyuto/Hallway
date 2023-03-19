using System;
using Mirror;
using UnityEngine;

namespace Network
{
    public class LocalPlayer : NetworkBehaviour
    {
        [SyncVar, SerializeField] private float playerDistance;
        public float PlayerDistance => playerDistance;
        
        public static LocalPlayer Player;

        public Action OnLocalPlayerInit;

        private void Awake()
        {
            Player = this;
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            OnLocalPlayerInit?.Invoke();
        }

        [Command(requiresAuthority = false)]
        public void CmdUpdatePlayerDistance(float newDistance)
        {
            if (newDistance < 0) newDistance = 0;
            playerDistance = newDistance;
        }
    }
}