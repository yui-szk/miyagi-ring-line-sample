using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public Image fadeImage;

    [Header("Train SE")]
    public AudioClip trainDoorSE;

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

    // 外から呼ぶ
    public void FadeAndLoadScene(string sceneName, float fadeOutTime, float fadeInTime, float waitTime)
    {
        StartCoroutine(Fade(sceneName, fadeOutTime, fadeInTime, waitTime));
    }

    IEnumerator Fade(string sceneName, float fadeOut, float fadeIn, float wait)
    {
        // 🚃 電車シーン判定（タグ方式）
        if (GameObject.FindGameObjectWithTag("TrainScene") != null)
        {
            if (trainDoorSE != null)
            {
                AudioSource.PlayClipAtPoint(trainDoorSE, Vector3.zero);
            }
        }

        // フェードアウト
        for (float t = 0; t < fadeOut; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(0f, 1f, t / fadeOut);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1f);

        // 少し待つ
        yield return new WaitForSeconds(wait);

        // シーン移動
        SceneManager.LoadScene(sceneName);

        yield return null;

        // フェードイン
        for (float t = 0; t < fadeIn; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(1f, 0f, t / fadeIn);
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0f);
    }
}