using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommentController : MonoBehaviour
{
    [Header("Comment Images")]
    public Image lowScoreComment;
    public Image midScoreComment;
    public Image highScoreComment;

    [Header("Score Borders")]
    public int midScoreBorder = 50;
    public int highScoreBorder = 100;

    [Header("Timing")]
    public float showDelay = 0.8f;

    void Start()
    {
        // 🔹 位置・サイズ・スケールは触らない
        lowScoreComment.gameObject.SetActive(false);
        midScoreComment.gameObject.SetActive(false);
        highScoreComment.gameObject.SetActive(false);

        StartCoroutine(ShowCommentDelayed());
    }

    IEnumerator ShowCommentDelayed()
    {
        yield return new WaitForSeconds(showDelay);

        int score = GameManager_Sand.FinalScore;

        if (score >= highScoreBorder)
            highScoreComment.gameObject.SetActive(true);
        else if (score >= midScoreBorder)
            midScoreComment.gameObject.SetActive(true);
        else
            lowScoreComment.gameObject.SetActive(true);
    }
}
