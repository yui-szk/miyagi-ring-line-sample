using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToTrainController : MonoBehaviour
{
    Joycon joycon;

    [Header("Return UI")]
    [Tooltip("Aボタン操作を解禁するための画像")]
    public Image returnGuideImage;

    bool canReturnToTrain = false;

    void Start()
    {
        if (JoyconManager.Instance != null && JoyconManager.Instance.j.Count > 0)
        {
            joycon = JoyconManager.Instance.j[0];
        }

        // 最初は画像を非表示
       // if (returnGuideImage != null)
        //{
        //    returnGuideImage.gameObject.SetActive(false);
        //}
    }

    void Update()
    {
        if (joycon == null)
        {
            Debug.Log("❌ joycon null");
            return;
        }

        if (!canReturnToTrain)
        {
            Debug.Log("⏸ canReturnToTrain = false");
            return;
        }

        if (joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            Debug.Log("🎮 Aボタン検知");
            ReturnToTrain();
        }
    }


    // 🔽 画像表示 ＋ Aボタン解禁
    public void ShowReturnGuide()
    {
        Debug.Log("🔥 ShowReturnGuide 呼ばれた");

        if (returnGuideImage == null)
        {
            Debug.LogWarning("⚠ returnGuideImage が未設定");
            return;
        }

        returnGuideImage.gameObject.SetActive(true);
        canReturnToTrain = true;

        Debug.Log("🖼 案内画像表示 → Aボタンで電車に戻れる");
    }

    void ReturnToTrain()
    {
        string trainScene = "";

        switch (GameStateManager.Instance.currentSeason)
        {
            case Season.Spring:
                trainScene = "train0";
                break;
            case Season.Summer:
                trainScene = "train1";
                break;
            case Season.Autumn:
                trainScene = "train2";
                break;
            case Season.Winter:
                trainScene = "train3";
                break;
        }

        Debug.Log("🚃 電車シーンへ戻る: " + trainScene);
        SceneManager.LoadScene(trainScene);
    }
}
