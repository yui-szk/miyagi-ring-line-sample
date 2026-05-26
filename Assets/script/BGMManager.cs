using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    AudioSource bgmSource;

    void Awake()
    {
        // ìÒèdê∂ê¨ñhé~
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = GetComponent<AudioSource>();
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        // ìØÇ∂BGMÇ»ÇÁçƒê∂ÇµÇ»Ç¢
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetVolume(float volume)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = volume;
        }
    }
}