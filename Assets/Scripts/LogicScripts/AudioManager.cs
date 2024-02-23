using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource failAudioSource; // Assign in inspector
    public AudioSource winAudioSource; // Assign in inspector

    public void PlayWinSound() {
        winAudioSource.Play();
    }

    public void PlayFailSound()
    {
        failAudioSource.Play();
    }
}
