using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainScene : MonoBehaviour
{
    Joycon joycon;

    [SerializeField] AudioClip buttonSE;//0527追加

    public Transform player;
    public AudioClip doorSE;

    public float sceneChangeDelay = 2.0f;

    bool isChangingScene = false;

    void Start()
    {
        if (JoyconManager.Instance == null || JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("❌ JoyconManager または Joycon が見つかりません");
            return;
        }

        joycon = JoyconManager.Instance.j[0];
        Debug.Log("✅ Joycon assigned in MainScene");
    }

    void Update()
    {
        if (joycon == null) return;

        if (!isChangingScene && joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            Debug.Log("🎮 Aボタン押された");

            isChangingScene = true;

            // ★ここ
            SEManager.Instance.PlaySE(buttonSE);//0527追加

            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        // 🚃電車減速
        TrainSpline train = FindObjectOfType<TrainSpline>();
        if (train != null)
        {
            train.StopTrain();
        }

        yield return new WaitForSeconds(3.5f);

        // 🚪ドアSE
        if (SEManager.Instance != null)
        {
            SEManager.Instance.PlaySE(doorSE);
        }

        yield return new WaitForSeconds(sceneChangeDelay);

        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "";

        if (player != null && GameStateManager.Instance != null)
        {
            GameStateManager.Instance.playerPosition = player.position;
            Debug.Log("📍 位置保存: " + player.position);
        }

        switch (currentScene)
        {
            case "train0":
                GameStateManager.Instance.currentSeason = Season.Spring;
                nextScene = "Talk_Take";
                break;

            case "train1":
                GameStateManager.Instance.currentSeason = Season.Summer;
                nextScene = "Talk_Garasu";
                break;

            case "train2":
                GameStateManager.Instance.currentSeason = Season.Autumn;
                nextScene = "Talk_Washi";
                break;

            case "train3":
                GameStateManager.Instance.currentSeason = Season.Winter;
                nextScene = "Talk_Fude_Suzuri";
                break;

            default:
                Debug.LogWarning("未定義のシーンからの遷移です: " + currentScene);
                yield break;
        }

        Debug.Log($"➡ シーン遷移: {currentScene} → {nextScene}");

        SceneManager.LoadScene(nextScene);
    }
}