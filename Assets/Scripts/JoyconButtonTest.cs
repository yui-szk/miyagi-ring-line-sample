using UnityEngine;

public class JoyconButtonTest : MonoBehaviour
{
    Joycon joycon;

    void Start()
    {
        if (JoyconManager.Instance == null)
        {
            Debug.LogError("JoyconManager がありません");
            return;
        }

        if (JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("Joy-Con が見つかりません");
            return;
        }

        joycon = JoyconManager.Instance.j[0];
        Debug.Log("Joy-Con 検出成功（全ボタンテスト開始）");
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
