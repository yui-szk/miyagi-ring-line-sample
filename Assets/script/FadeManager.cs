using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("Fade")]
    public Image fadeImage;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip tunnelSE;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // フェード付きシーン遷移
    public void FadeAndLoadScene(string sceneName, float fadeOutTime, float fadeInTime, float waitTime)
    {
        StartCoroutine(Fade(sceneName, fadeOutTime, fadeInTime, waitTime));
    }

    IEnumerator Fade(string sceneName, float fadeOut, float fadeIn, float wait)
    {
        // トンネル音を再生
        if (audioSource != null && tunnelSE != null)
        {
            audioSource.clip = tunnelSE;
            audioSource.Play();
        }

        // フェードアウト
        for (float t = 0; t < fadeOut; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(0f, 1f, t / fadeOut);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1f);

        // 真っ暗のまま待機
        yield return new WaitForSeconds(wait);

        // シーン切り替え
        SceneManager.LoadScene(sceneName);

        // シーン読み込み待ち
        yield return null;

        // フェードイン
        for (float t = 0; t < fadeIn; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(1f, 0f, t / fadeIn);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0f);

        // トンネル音を停止
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}