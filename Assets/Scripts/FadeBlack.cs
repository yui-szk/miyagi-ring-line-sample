using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class FadeBlack : MonoBehaviour
{
    [SerializeField] private Image FadeBlackPanel;
    [SerializeField] private GameObject TextWindow;
    [SerializeField] private GameObject NameWindow;
    [SerializeField] private TextMeshProUGUI NAME;

    public float displayDelay_SceneStart = 2.0f;
    private bool isDisplayed;

    private float FadeTime = 0.8f;

    private IEnumerator Fade_Black(float StartBlack, float EndBlack, System.Action onComplete = null)
    {
        float time_Black = 0f;

        while (time_Black < FadeTime)
        {
            time_Black += Time.deltaTime;
            float alpha_Black = Mathf.Lerp(StartBlack, EndBlack, time_Black / FadeTime);
            SetAlpha(alpha_Black);
            yield return null;
        }
        SetAlpha(EndBlack);
        onComplete?.Invoke();
    }

    private void SetAlpha(float a)
    {
        Color c = FadeBlackPanel.color;
        c.a = a;
        FadeBlackPanel.color = c;
    }

    void FadeOut_SceneStart(Action onComplete = null)
    {
        StartCoroutine(Fade_Black(1f, 0f, onComplete));
    }

    public void FadeIn_SceneEnd(Action onComplete = null)
    {
        TextWindow.SetActive(false);
        NameWindow.SetActive(false);
        NAME.gameObject.SetActive(false);
        StartCoroutine(Fade_Black(0f, 1f, onComplete));
    }

    void Start()
    {
        TextWindow.SetActive(false);
        NameWindow.SetActive(false);
        NAME.gameObject.SetActive(false);
        SetAlpha(1f);
        //FadeOut_SceneStart();
        FadeOut_SceneStart(() =>
        {
            StartCoroutine(DisplayWIndows());
        });
    }

    private IEnumerator DisplayWIndows()
    {
        yield return new WaitForSeconds(displayDelay_SceneStart);
        TextWindow.SetActive(true);
        NameWindow.SetActive(true);
        NAME.gameObject.SetActive(true);
        isDisplayed = true;
    }
}
