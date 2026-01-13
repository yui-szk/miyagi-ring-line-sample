using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class TunnelTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeOutTime = 1.5f;
    public float darkTime = 2.5f;
    public float fadeInTime = 1.5f;
    public AudioSource tunnelAudio;
    public string nextSceneName;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    public void StartTunnelSequence()
    {
        StartCoroutine(TunnelRoutine());
    }

    IEnumerator TunnelRoutine()
    {
        yield return Fade(0f, 1f, fadeOutTime);

        if (tunnelAudio)
            tunnelAudio.Play();

        yield return new WaitForSeconds(darkTime);

        SceneManager.LoadScene(nextSceneName);

        yield return null; // 1ÉtÉåÅ[ÉÄë“Ç¬

        yield return Fade(1f, 0f, fadeInTime);
    }

    IEnumerator Fade(float from, float to, float time)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < time)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / time);
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
    }
}
