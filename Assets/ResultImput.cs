using UnityEngine;

public class ResultInput : MonoBehaviour
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
        // Xボタン（上）で STARTへ
        if (joycon.GetButtonDown(Joycon.Button.DPAD_UP))
        {
            GameManager.Instance.ShowStart();
        }
    }
}
