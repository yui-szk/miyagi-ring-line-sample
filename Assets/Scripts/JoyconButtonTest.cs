using UnityEngine;

public class JoyconButtonTest : MonoBehaviour
{
    Joycon joycon;

    void Start()
    {
        if (JoyconManager.Instance == null)
        {
            Debug.LogError("JoyconManager ������܂���");
            return;
        }

        if (JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("Joy-Con ��������܂���");
            return;
        }

        joycon = JoyconManager.Instance.j[0];
        Debug.Log("Joy-Con ���o�����i�S�{�^���e�X�g�J�n�j");
    }

    void Update()
    {
        if (joycon == null) return;

        foreach (Joycon.Button b in System.Enum.GetValues(typeof(Joycon.Button)))
        {
            if (joycon.GetButtonDown(b))
            {
                Debug.Log($"Button Down: {b}");
            }
        }
    }
}
