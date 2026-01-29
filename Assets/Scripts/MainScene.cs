using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    Joycon joycon;

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
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string nextScene = "";

        switch (currentScene)
        {
            case "train0":
                nextScene = "Talk_Take";
                break;
            case "train1":
                nextScene = "Talk_Garasu";
                break;
            case "train2":
                nextScene = "Talk_Washi";
                break;
            case "train3":
                nextScene = "Talk_Fude_Suzuri";
                break;
            default:
                Debug.LogWarning("未定義のシーンからの遷移です: " + currentScene);
                return;
        }

        Debug.Log("➡ シーン遷移: " + currentScene + " → " + nextScene);
        SceneManager.LoadScene(nextScene);
    }
}
