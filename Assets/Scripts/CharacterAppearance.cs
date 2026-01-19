using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAppearance : MonoBehaviour
{
    [SerializeField] private Image AppearanceCharacter;
    private float fadeDuration = 0.8f;

    void FadeIn()
    {
        StartCoroutine(Fade(0, 1));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0f;
        Color fadeColor = AppearanceCharacter.color;

        while(time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            fadeColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
            AppearanceCharacter.color = fadeColor;
            yield return null;
        }

        fadeColor.a = endAlpha;
        AppearanceCharacter.color = fadeColor;
    }

    void Start()
    {
        Color c = AppearanceCharacter.color;
        c.a = 0f;
        AppearanceCharacter.color = c;
        FadeIn();
    }
}
