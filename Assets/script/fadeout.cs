using UnityEngine;
using UnityEngine.UI;

public class FadeOutOnEnter : MonoBehaviour
{
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    float alpha = 0f;
    bool isFading = false;

    void Start()
    {
        // îOÇÃÇΩÇﬂèâä˙âª
        fadeImage.color = new Color(0, 0, 0, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            isFading = true;
        }

        if (isFading)
        {
            alpha += Time.deltaTime * fadeSpeed;
            alpha = Mathf.Clamp01(alpha);
            fadeImage.color = new Color(0, 0, 0, alpha);
        }
    }
}
