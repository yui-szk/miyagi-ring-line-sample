using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager2 : MonoBehaviour
{
    public static GameManager2 Instance;

    [Header("Game Settings")]
    public float timeLimit = 30f;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject startUI;   // START! ï\é¶
    public GameObject endUI;

    [Header("Objects")]
    public SakuraSpawner sakuraSpawner; // Åö SakuraSpawner Ç…ïœçX

    // ===== åãâ óp =====
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

        // â‘Ç—ÇÁÇÕç≈èâé~ÇﬂÇÈ
        if (sakuraSpawner != null)
            sakuraSpawner.StopSpawn();

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

        // START! ï\é¶
        if (startUI != null)
            startUI.SetActive(true);

        // Åö ç˜ÇÃâ‘Ç—ÇÁÇç~ÇÁÇπÇÈ
        if (sakuraSpawner != null)
            sakuraSpawner.StartSpawn();

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

        // â‘Ç—ÇÁÇé~ÇﬂÇÈ
        if (sakuraSpawner != null)
            sakuraSpawner.StopSpawn();

        if (endUI != null)
            endUI.SetActive(true);

        FinalScore = score;

        Invoke(nameof(LoadResult), 2f);
    }

    void LoadResult()
    {
        SceneManager.LoadScene("result_sakura_3types");
    }
}
