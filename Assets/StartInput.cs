using UnityEngine;

public class StartInput : MonoBehaviour
{
    Joycon joycon;

    void Start()
    {
        if (JoyconManager.Instance == null || JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("Joy-Con がありません");
            enabled = false;
            return;
        }
        joycon = JoyconManager.Instance.j[0];
    }

    void Update()
    {
        // Aボタン（右）で GAMEへ
        if (joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            GameManager.Instance.ShowGame();
        }
    }
}
