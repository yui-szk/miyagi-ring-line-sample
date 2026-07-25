/*using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText; 
    public int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (scoreText == null)
        {
            GameObject textObj = GameObject.Find("ScoreText");
            if (textObj != null)
            {
                scoreText = textObj.GetComponent<TextMeshProUGUI>();
            }
        }
        
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "score: " + score;
        }
        Debug.Log("Current Score: " + score);
    }
}
*/

using UnityEngine;
using UnityEngine.SceneManagement;  // ← 追加
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText;
    public int score = 0;

    // ↓ 追加 ここから
    [Header("Timer")]
    public float timeLimit = 60f;
    public TextMeshProUGUI timerText;
    public string nextSceneName = "washi_result";

    private float timeRemaining;
    private bool isGameOver = false;
    // ↑ 追加 ここまで

    void Awake() { /* 変更なし */ }

    void Start()
    {
        // 既存コード（scoreText の取得）はそのまま

        // ↓ 追加
        timeRemaining = timeLimit;
        UpdateTimerText();
    }

    // ↓ 追加
    void Update()
    {
        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            isGameOver = true;
            UpdateTimerText();
            Debug.Log("⏰ 時間切れ！シーン遷移: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            UpdateTimerText();
        }
    }

    public void AddScore(int amount) { /* 変更なし */ }
    void UpdateScoreText() { /* 変更なし */ }

    // ↓ 追加
    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.CeilToInt(timeRemaining);
        }
    }
}