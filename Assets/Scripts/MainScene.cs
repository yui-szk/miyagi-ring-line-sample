using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    Joycon joycon;

    void Start()
    {
        joycon = JoyconManager.Instance.j[0];
    }

    void Update()
    {
        // Aボタン（JoyconLibでは DPAD_RIGHT）
        if (joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            Debug.Log("🎮 Aボタン押された");
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        SceneManager.LoadScene("Talk_Take");
    }
}
