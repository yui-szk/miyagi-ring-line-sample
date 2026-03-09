using UnityEngine;
using TMPro;
using System.Collections;

public class ResultSceneManager : MonoBehaviour
{
    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

    [Header("Slide Root")]
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

    [Header("Result Sounds")]
    public AudioSource audioSource;
    public AudioClip lowScoreSound;
    public AudioClip midScoreSound;
    public AudioClip highScoreSound;
    public float resultSoundDelay = 0f;

    [Header("BGM Volume")]
    [Range(0f, 1f)]
    public float resultBgmVolume = 0.2f;

    void Start()
    {

        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.SetVolume(0.3f);
        }

        scoreText.gameObject.SetActive(false);

        if (lowScoreObj != null) lowScoreObj.SetActive(false);
        if (midScoreObj != null) midScoreObj.SetActive(false);
        if (highScoreObj != null) highScoreObj.SetActive(false);

        scoreText.text = GameManager_Sand.FinalScore.ToString();



        StartCoroutine(ResultSequence());
    }

    IEnumerator ResultSequence()
    {
        yield return new WaitForSeconds(0.8f);

        scoreText.gameObject.SetActive(true);
        yield return StartCoroutine(PunchFontSize(scoreText));

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(Slide());

        ShowResultByScore();
    }

    void ShowResultByScore()
    {
        int score = GameManager_Sand.FinalScore;

        if (score >= highScoreBorder)
        {
            if (highScoreObj != null)
                highScoreObj.SetActive(true);

            StartCoroutine(MoveCamera(highCameraPos));
            StartCoroutine(PlayResultSound(highScoreSound));
        }
        else if (score >= midScoreBorder)
        {
            if (midScoreObj != null)
                midScoreObj.SetActive(true);

            StartCoroutine(MoveCamera(midCameraPos));
            StartCoroutine(PlayResultSound(midScoreSound));
        }
        else
        {
            if (lowScoreObj != null)
                lowScoreObj.SetActive(true);

            StartCoroutine(MoveCamera(lowCameraPos));
            StartCoroutine(PlayResultSound(lowScoreSound));
        }
    }

    IEnumerator PlayResultSound(AudioClip clip)
    {
        if (audioSource == null || clip == null) yield break;

        yield return new WaitForSeconds(resultSoundDelay);

        audioSource.PlayOneShot(clip);
    }

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