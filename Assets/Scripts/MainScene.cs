using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainScene : MonoBehaviour
{
    Joycon joycon;
    public Transform player; // ← インスペクターで指定
    public AudioClip doorSE; // 🚪ドア音

    public float sceneChangeDelay = 2.0f; // ⏳ シーン移動までの待ち時間


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

        // Aボタン（JoyconLibでは DPAD_RIGHT）
        if (joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            Debug.Log("🎮 Aボタン押された");
            StartCoroutine(ChangeScene());
        }
    }


    IEnumerator ChangeScene()
    {
        // 🚪ドア音
        if (SEManager.Instance != null)
        {
            SEManager.Instance.PlaySE(doorSE);
        }

        // ⏳待ち時間
        yield return new WaitForSeconds(sceneChangeDelay);


        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "";


        // 🔽 位置保存
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

        Debug.Log(
            $"➡ シーン遷移: {currentScene} → {nextScene} / Season: {GameStateManager.Instance.currentSeason}"
        );

        SceneManager.LoadScene(nextScene);
    }
}