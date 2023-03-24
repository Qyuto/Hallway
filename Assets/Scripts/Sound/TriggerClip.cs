using UnityEngine;

namespace Sound
{
    public class TriggerClip : MonoBehaviour
    {
        [SerializeField] private bool onePlay = true;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip clip;
        [Range(1, 100), SerializeField] private int playClipPercent = 50;
        private bool _isPlayed;

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag("Player")) return;
            if (onePlay && _isPlayed) return;
            if (playClipPercent != 100)
            {
                int currentPercent = Random.Range(0, 100);
                if (currentPercent > playClipPercent) return;
            }

            audioSource.clip = clip;
            audioSource.Play();
            _isPlayed = true;
        }
    }
}