using UnityEngine;

public class StartInput : MonoBehaviour
{
    Joycon joycon;

    void Start()
    {
        if (JoyconManager.Instance == null || JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("Joy-Con ������܂���");
            enabled = false;
            return;
        }
        joycon = JoyconManager.Instance.j[0];
    }

    void Update()
    {
        // A�{�^���i�E�j�� GAME��
        if (joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            GameManager.Instance.ShowGame();
        }
    }
}
