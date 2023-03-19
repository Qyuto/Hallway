using Mirror;
using UnityEngine;

namespace Sound
{
    public class TriggerSound : NetworkBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private bool onePlay = true;
        [Range(1, 100),SerializeField] private int playClipPercent = 50;
        [SerializeField] private AudioClip[] clips;


        private bool _isPlayed;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            if(!other.GetComponent<NetworkIdentity>().isLocalPlayer) return;
            if (onePlay && _isPlayed) return;
            if(playClipPercent != 100)
            {
                int currentPercent = Random.Range(0, 100);
                if(currentPercent > playClipPercent) return;
            }
            

            CmdPlayClipId(Random.Range(0, clips.Length));
            _isPlayed = true;
        }

        [Command(requiresAuthority = false)]
        private void CmdPlayClipId(int localClipId)
        {
            ClientPlayClip(localClipId);
        }

        [ClientRpc]
        private void ClientPlayClip(int localClipId)
        {
            audioSource.clip = clips[localClipId];
            audioSource.Play();
        }
    }
}