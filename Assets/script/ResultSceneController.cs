using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneController : MonoBehaviour
{
    Joycon joycon;

    void Start()
    {
        if (JoyconManager.Instance != null && JoyconManager.Instance.j.Count > 0)
        {
            joycon = JoyconManager.Instance.j[0];
        }
    }

    void Update()
    {
        if (joycon == null) return;

        if (joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            ReturnToTrain();
        }
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

        Debug.Log("🚃 リザルト → 電車: " + trainScene);
        SceneManager.LoadScene(trainScene);
    }
}
