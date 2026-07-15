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
    public GameObject startUI;
    public GameObject endUI;

    [Header("Objects")]
    public SandSpawner sandSpawner;

    [Header("Game BGM")]
    [SerializeField] AudioClip gameBGM;

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

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 会話BGMを停止してゲームBGMを再生
        if (BGMManager.Instance != null)
        {
            BGMManager.Instance.StopBGM();
            BGMManager.Instance.PlayBGM(gameBGM);
        }

        gameActive = false;
        currentTime = timeLimit;

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

    public void StartGame()
    {
        gameActive = true;
        currentTime = timeLimit;

        if (startUI != null)
            startUI.SetActive(true);

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

    void FinishGame()
    {
        gameActive = false;

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