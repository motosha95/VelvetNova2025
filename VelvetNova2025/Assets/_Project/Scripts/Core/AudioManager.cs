using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip flipClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;
    public AudioClip gameOverClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFlip() => PlaySound(flipClip);
    public void PlayMatch() => PlaySound(matchClip);
    public void PlayMismatch() => PlaySound(mismatchClip);
    public void PlayGameOver() => PlaySound(gameOverClip);

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
