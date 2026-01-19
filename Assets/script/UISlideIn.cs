using UnityEngine;
using System.Collections;

public class UISlideIn : MonoBehaviour
{
    [Header("スライド設定")]
    public Vector2 startOffset = new Vector2(-800f, 0f); // 開始位置（左から）
    public float slideDuration = 0.5f;                  // スライド時間
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    RectTransform rectTransform;
    Vector2 targetPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        targetPos = rectTransform.anchoredPosition;
    }

    void OnEnable()
    {
        // 初期位置をずらす
        rectTransform.anchoredPosition = targetPos + startOffset;

        // スライド開始
        StartCoroutine(SlideIn());
    }

    IEnumerator SlideIn()
    {
        float time = 0f;
        Vector2 startPos = rectTransform.anchoredPosition;

        while (time < slideDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / slideDuration);
            float easedT = ease.Evaluate(t);

            rectTransform.anchoredPosition =
                Vector2.Lerp(startPos, targetPos, easedT);

            yield return null;
        }

        rectTransform.anchoredPosition = targetPos;
    }
}
