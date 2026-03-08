using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;   // Å© í«â¡

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
        bgmSource.loop = true;
    }

    public void PlayBGM(AudioClip clip)
    {
        // ìØÇ∂BGMÇ»ÇÁçƒê∂ÇµÇ»Ç¢
        if (bgmSource.clip == clip) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
}