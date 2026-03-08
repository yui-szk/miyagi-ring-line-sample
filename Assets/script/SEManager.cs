using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance;

    AudioSource seSource;

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

    public void PlaySE(AudioClip clip)
    {
        if (clip == null) return;

        seSource.PlayOneShot(clip);
    }
}