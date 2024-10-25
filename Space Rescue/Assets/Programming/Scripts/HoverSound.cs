using UnityEngine;

public class HoverSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip hoverSound;

    public void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }
}