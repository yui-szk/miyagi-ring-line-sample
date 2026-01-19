using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager_Sand : MonoBehaviour
{
    public static GameManager_Sand Instance;

    [Header("Game Settings")]
    public float timeLimit = 30f;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject startUI;   // START! 表示
    public GameObject endUI;

    [Header("Objects")]
    public SandSpawner sandSpawner;

    // ===== 結果用 =====
    public static int FinalScore;

    int score = 0;
    float currentTime;
    bool gameActive = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        gameActive = false;
        currentTime = timeLimit;

        // 砂は最初止める
        if (sandSpawner != null)
            sandSpawner.StopSpawn();

        if (startUI != null)
            startUI.SetActive(false);

        if (endUI != null)
            endUI.SetActive(false);

        UpdateScoreUI();
        UpdateTimerUI();
    }

    void Update()
    {
        if (!gameActive) return;

        currentTime -= Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            FinishGame();
        }
    }

    // =============================
    public void StartGame()
    {
        gameActive = true;
        currentTime = timeLimit;

        // START! 表示
        if (startUI != null)
            startUI.SetActive(true);

        // ★ ここが最重要：砂を開始
        if (sandSpawner != null)
            sandSpawner.StartSpawn();

        UpdateScoreUI();
        UpdateTimerUI();

        Invoke(nameof(HideStartUI), 1.0f);
    }

    void HideStartUI()
    {
        if (startUI != null)
            startUI.SetActive(false);
    }

    // =============================
    public void AddScore(int amount = 1)
    {
        if (!gameActive) return;

        score += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score : " + score;
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = "Time : " + Mathf.CeilToInt(currentTime);
    }

    // =============================
    void FinishGame()
    {
        gameActive = false;

        // 砂を止める
        if (sandSpawner != null)
            sandSpawner.StopSpawn();

        if (endUI != null)
            endUI.SetActive(true);

        FinalScore = score;

        Invoke(nameof(LoadResult), 2f);
    }

    void LoadResult()
    {
        SceneManager.LoadScene("result_3types");
    }
}
