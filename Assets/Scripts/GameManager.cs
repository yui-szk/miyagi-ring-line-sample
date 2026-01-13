using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Canvas")]
    public GameObject canvasStart;
    public GameObject canvasGame;
    public GameObject canvasResult;

    [Header("Result")]
    public float lastAccuracy = 0f;

    [Header("DrawLine")]
    public JoyconDrawLine drawLine; // 未設定でも自動取得

    [Header("Kanji")]
    public Image kanjiImage;
    public Sprite[] kanjiSprites;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // ★未設定なら自動取得（非アクティブも含める）
        if (drawLine == null)
            drawLine = Object.FindFirstObjectByType<JoyconDrawLine>(FindObjectsInactive.Include);
    }

    void Start()
    {
        ShowStart();
    }

    public void SetResult(float accuracy)
    {
        lastAccuracy = accuracy;
    }

    public void ShowResult()
    {
        canvasStart.SetActive(false);
        canvasGame.SetActive(false);
        canvasResult.SetActive(true);
    }

    public void ShowResult(float accuracy)
    {
        lastAccuracy = accuracy;
        ShowResult();
    }

    public void ShowStart()
    {
        if (drawLine == null)
            drawLine = Object.FindFirstObjectByType<JoyconDrawLine>(FindObjectsInactive.Include);

        // ★STARTに戻ったら線を全消し
        if (drawLine != null)
            drawLine.ClearAllLines();

        canvasStart.SetActive(true);
        canvasGame.SetActive(false);
        canvasResult.SetActive(false);
    }

    public void ShowGame()
    {
        // ★ゲーム開始のたびにランダム出題
        if (kanjiImage != null && kanjiSprites != null && kanjiSprites.Length > 0)
        {
            int idx = Random.Range(0, kanjiSprites.Length);
            kanjiImage.sprite = kanjiSprites[idx];
        }
        else
        {
            Debug.LogWarning("GameManager: kanjiImage か kanjiSprites が未設定です");
        }

        if (drawLine == null)
            drawLine = Object.FindFirstObjectByType<JoyconDrawLine>(FindObjectsInactive.Include);

        // ★新プレイ開始で線を全消し（必要なら残してOK）
        if (drawLine != null)
            drawLine.ClearAllLines();

        canvasStart.SetActive(false);
        canvasGame.SetActive(true);
        canvasResult.SetActive(false);
    }
}
