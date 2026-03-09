using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;

    AudioSource seSource;

    // íxâÑçƒê∂óp
    AudioClip delayedClip;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        seSource = GetComponent<AudioSource>();

        if (seSource == null)
        {
            seSource = gameObject.AddComponent<AudioSource>();
        }

        seSource.playOnAwake = false;
        seSource.loop = false;
    }

    // í èÌSEçƒê∂
    public void PlaySE(AudioClip clip)
    {
        if (clip == null) return;

        seSource.PlayOneShot(clip);
    }

    // ÅöíxâÑSEçƒê∂
    public void PlaySEDelay(AudioClip clip, float delay)
    {
        if (clip == null) return;

        delayedClip = clip;
        Invoke(nameof(PlaySE_Internal), delay);
    }

    void PlaySE_Internal()
    {
        if (delayedClip != null)
        {
            seSource.PlayOneShot(delayedClip);
        }
    }
}