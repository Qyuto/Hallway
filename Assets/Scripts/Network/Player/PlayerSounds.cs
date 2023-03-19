using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Network.Player
{
    [Serializable]
    public class Audio
    {
        public AudioClip clip;
        public string clipKey;
    }

    public class PlayerSounds : NetworkBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Audio[] _audios;
        
        private readonly Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            foreach (var cAudio in _audios)
                _audioClips.Add(cAudio.clipKey, cAudio.clip);
        }

        [Command]
        public void CmdPlaySound(string clipKey)
        {
            ClientPlaySound(clipKey);
        }

        [Command]
        public void CmdForceStopSound(string clipKey)
        {
            ClientForceStopSound(clipKey);
        }

        [ClientRpc]
        private void ClientPlaySound(string clipKey)
        {
            if (!_audioClips.TryGetValue(clipKey, out AudioClip clip)) return;
            audioSource.clip = clip;
            audioSource.Play();
        }
        
        [ClientRpc]
        private void ClientForceStopSound(string clipKey)
        {
            if (!_audioClips.TryGetValue(clipKey, out AudioClip clip)) return;
            audioSource.Stop();
        }
    }
}