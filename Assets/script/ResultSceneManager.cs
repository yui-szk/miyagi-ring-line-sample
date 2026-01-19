using UnityEngine;
using TMPro;
using System.Collections;

public class ResultSceneManager : MonoBehaviour
{
    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

    [Header("Slide Root（スライドさせる親）")]
    public RectTransform slideRoot;
    public float slideDistance = 1920f;
    public float slideDuration = 1.0f;

    [Header("Score Result Objects")]
    public GameObject lowScoreObj;
    public GameObject midScoreObj;
    public GameObject highScoreObj;

    [Header("Score Borders")]
    public int midScoreBorder = 50;
    public int highScoreBorder = 100;

    [Header("Camera Settings")]
    public Camera resultCamera;
    public Vector3 lowCameraPos = new Vector3(0, 2, -6);
    public Vector3 midCameraPos = new Vector3(0, 2, -8);
    public Vector3 highCameraPos = new Vector3(0, 2, -10);
    public float cameraMoveDuration = 1.0f;

    void Start()
    {
        // ---------- 初期化 ----------
        scoreText.gameObject.SetActive(false);

        if (lowScoreObj != null) lowScoreObj.SetActive(false);
        if (midScoreObj != null) midScoreObj.SetActive(false);
        if (highScoreObj != null) highScoreObj.SetActive(false);

        // スコア文字設定（まだ表示しない）
        scoreText.text = GameManager_Sand.FinalScore.ToString();

        StartCoroutine(ResultSequence());
    }

    // =============================
    IEnumerator ResultSequence()
    {
        // ① 少し待つ
        yield return new WaitForSeconds(0.8f);

        // ② 数字表示 → ドンッ
        scoreText.gameObject.SetActive(true);
        yield return StartCoroutine(PunchFontSize(scoreText));

        // ③ 間
        yield return new WaitForSeconds(0.5f);

        // ④ スライド
        yield return StartCoroutine(Slide());

        // ⑤ スコア別表示＆カメラ演出
        ShowResultByScore();
    }

    // =============================
    void ShowResultByScore()
    {
        int score = GameManager_Sand.FinalScore;

        if (score >= highScoreBorder)
        {
            if (highScoreObj != null)
                highScoreObj.SetActive(true);

            StartCoroutine(MoveCamera(highCameraPos));
        }
        else if (score >= midScoreBorder)
        {
            if (midScoreObj != null)
                midScoreObj.SetActive(true);

            StartCoroutine(MoveCamera(midCameraPos));
        }
        else
        {
            if (lowScoreObj != null)
                lowScoreObj.SetActive(true);

            StartCoroutine(MoveCamera(lowCameraPos));
        }
    }

    // =============================
    IEnumerator Slide()
    {
        Vector2 startPos = slideRoot.anchoredPosition;
        Vector2 endPos = startPos + Vector2.left * slideDistance;

        float t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            slideRoot.anchoredPosition =
                Vector2.Lerp(startPos, endPos, t / slideDuration);
            yield return null;
        }

        slideRoot.anchoredPosition = endPos;
    }

    // =============================
    // Scaleを触らず、fontSizeでドンッ
    IEnumerator PunchFontSize(TextMeshProUGUI text)
    {
        float baseSize = text.fontSize;
        float bigSize = baseSize * 1.3f;

        float t = 0f;
        while (t < 0.2f)
        {
            t += Time.deltaTime;
            text.fontSize = Mathf.Lerp(0f, bigSize, t / 0.2f);
            yield return null;
        }

        t = 0f;
        while (t < 0.1f)
        {
            t += Time.deltaTime;
            text.fontSize = Mathf.Lerp(bigSize, baseSize, t / 0.1f);
            yield return null;
        }

        text.fontSize = baseSize;
    }

    // =============================
    IEnumerator MoveCamera(Vector3 targetPos)
    {
        if (resultCamera == null) yield break;

        Vector3 startPos = resultCamera.transform.position;
        float t = 0f;

        while (t < cameraMoveDuration)
        {
            t += Time.deltaTime;
            resultCamera.transform.position =
                Vector3.Lerp(startPos, targetPos, t / cameraMoveDuration);
            yield return null;
        }

        resultCamera.transform.position = targetPos;
    }
}
