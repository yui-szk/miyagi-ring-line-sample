using UnityEngine;

public class GameInput : MonoBehaviour
{
    Joycon joycon;

    [Header("参照（未設定でもOK）")]
    public JoyconDrawLine drawLine;

    [Header("デバッグ")]
    public bool debugLog = false;

    void OnEnable()
    {
        if (JoyconManager.Instance == null || JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("GameInput: Joy-Con がありません");
            enabled = false;
            return;
        }
        joycon = JoyconManager.Instance.j[0];

        if (drawLine == null)
            drawLine = Object.FindFirstObjectByType<JoyconDrawLine>(FindObjectsInactive.Include);

        if (debugLog)
            Debug.Log($"GameInput Enabled: joycon={(joycon != null)} drawLine={(drawLine != null)}");
    }

    void Update()
    {
        if (joycon == null) return;

        // ✅ Y（左）= 直前の線を1本消す（Undo）
        if (joycon.GetButtonDown(Joycon.Button.DPAD_LEFT))
        {
            if (drawLine == null)
                drawLine = Object.FindFirstObjectByType<JoyconDrawLine>(FindObjectsInactive.Include);

            if (drawLine != null)
            {
                drawLine.UndoLastLine();
                if (debugLog) Debug.Log("GameInput: UndoLastLine()");
            }
            return;
        }

        // ✅ R（SHOULDER_1）= 全消し（リセット）
        if (joycon.GetButtonDown(Joycon.Button.SHOULDER_1))
        {
            if (drawLine == null)
                drawLine = Object.FindFirstObjectByType<JoyconDrawLine>(FindObjectsInactive.Include);

            if (drawLine != null)
            {
                drawLine.ClearAllLines();
                if (debugLog) Debug.Log("GameInput: ClearAllLines()");
            }
        }
    }
}
