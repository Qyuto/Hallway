using UnityEngine;


public class PlayerStepSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip stepClip;

    public void Step()
    {
        audioSource.clip = stepClip;
        audioSource.Play();
    }
}