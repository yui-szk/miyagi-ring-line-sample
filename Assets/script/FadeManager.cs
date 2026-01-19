using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public Image fadeImage;

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

    // ← これを呼ぶ
    public void FadeAndLoadScene(string sceneName, float fadeOutTime, float fadeInTime, float waitTime)
    {
        StartCoroutine(Fade(sceneName, fadeOutTime, fadeInTime, waitTime));
    }

    // ← 実際に動く中身
    IEnumerator Fade(string sceneName, float fadeOut, float fadeIn, float wait)
    {
        // フェードアウト（速く）
        for (float t = 0; t < fadeOut; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(0f, 1f, t / fadeOut);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1f);

        yield return new WaitForSeconds(wait);

        SceneManager.LoadScene(sceneName);

        yield return null;

        // フェードイン（少しゆっくり）
        for (float t = 0; t < fadeIn; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(1f, 0f, t / fadeIn);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0f);
    }
}
