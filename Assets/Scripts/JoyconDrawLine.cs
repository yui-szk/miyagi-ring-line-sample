using UnityEngine;
using System.Collections.Generic;

public class JoyconDrawLine : MonoBehaviour
{
    Joycon joycon;

    [Header("参照")]
    public Transform pointerTransform;
    public Material lineMaterial;

    [Header("描画設定")]
    public float minDistance = 0.02f;
    public float lineWidth = 0.1f;


    [Header("レイヤー設定")]
    public string sortingLayerName = "Default";
    public int sortingOrder = 100;

    [Header("デバッグ")]
    public bool debugLog = false;

    List<Vector3> currentPoints = new List<Vector3>();
    LineRenderer currentLine;
    bool drawing = false;

    List<LineRenderer> allLines = new List<LineRenderer>();

    Queue<Vector3> smoothBuffer = new Queue<Vector3>();
    const int smoothCount = 3;

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
        if (joycon == null || pointerTransform == null) return;

        // ---------------------------
        // ✅ 入力（ここで確実に拾う）
        // ---------------------------

        // Y（左）= 直前の線を1本消す（Undo）
        if (joycon.GetButtonDown(Joycon.Button.DPAD_LEFT))
        {
            if (debugLog) Debug.Log("JoyconDrawLine: Y(DPAD_LEFT) Undo");
            UndoLastLine();
            return; // 同フレームで描画処理と混ざらないように
        }



        // ---------------------------
        // 描画処理
        // ---------------------------

        Vector3 rawPos = pointerTransform.position;
        Vector3 pointerPos = GetSmoothedPosition(rawPos);

        // ZR 押した瞬間 → 新しい線
        if (joycon.GetButtonDown(Joycon.Button.SHOULDER_2))
        {
            StartNewLine(pointerPos);
        }

        // ZR 押している間 → 線を伸ばす
        if (drawing && joycon.GetButton(Joycon.Button.SHOULDER_2))
        {
            if (currentPoints.Count == 0 ||
                Vector3.Distance(currentPoints[currentPoints.Count - 1], pointerPos) > minDistance)
            {
                AddPoint(pointerPos);
            }
        }

        // ZR 離した → 線を確定
        if (joycon.GetButtonUp(Joycon.Button.SHOULDER_2))
        {
            drawing = false;
            currentLine = null;
            currentPoints.Clear();
            smoothBuffer.Clear();
        }
    }

    Vector3 GetSmoothedPosition(Vector3 newPos)
    {
        smoothBuffer.Enqueue(newPos);
        if (smoothBuffer.Count > smoothCount)
            smoothBuffer.Dequeue();

        Vector3 sum = Vector3.zero;
        foreach (var p in smoothBuffer)
            sum += p;

        return sum / smoothBuffer.Count;
    }

    void StartNewLine(Vector3 startPos)
    {
        GameObject lineObj = new GameObject("DrawLine");
        lineObj.transform.parent = transform;

        currentLine = lineObj.AddComponent<LineRenderer>();
        currentLine.material = lineMaterial;
        currentLine.startWidth = lineWidth;
        currentLine.endWidth = lineWidth;
        currentLine.useWorldSpace = true;
        currentLine.positionCount = 0;

        currentLine.sortingLayerName = sortingLayerName;
        currentLine.sortingOrder = sortingOrder;

        currentLine.startColor = Color.red;
        currentLine.endColor = Color.red;

        currentLine.numCapVertices = 5;
        currentLine.numCornerVertices = 5;

        allLines.Add(currentLine);

        currentPoints.Clear();
        smoothBuffer.Clear();
        smoothBuffer.Enqueue(startPos);

        drawing = true;
        AddPoint(startPos);
    }

    void AddPoint(Vector3 pos)
    {
        currentPoints.Add(pos);
        currentLine.positionCount = currentPoints.Count;
        currentLine.SetPositions(currentPoints.ToArray());
    }

    // ★ 判定用：すべての線の点を返す
    public List<Vector3> GetAllPoints()
    {
        List<Vector3> all = new List<Vector3>();

        foreach (var line in allLines)
        {
            if (line == null) continue;

            Vector3[] positions = new Vector3[line.positionCount];
            line.GetPositions(positions);
            all.AddRange(positions);
        }

        return all;
    }

    // ★全消し
    public void ClearAllLines()
    {
        for (int i = 0; i < allLines.Count; i++)
        {
            if (allLines[i] != null)
                Destroy(allLines[i].gameObject);
        }
        allLines.Clear();

        drawing = false;
        currentLine = null;
        currentPoints.Clear();
        smoothBuffer.Clear();
    }

    // ★最後の線を1本だけ消す（Undo）
    public void UndoLastLine()
    {
        if (allLines.Count == 0) return;

        int last = allLines.Count - 1;
        LineRenderer lr = allLines[last];

        // 描画中の線を消す場合
        if (drawing && lr == currentLine)
        {
            drawing = false;
            currentLine = null;
            currentPoints.Clear();
            smoothBuffer.Clear();
        }

        if (lr != null)
            Destroy(lr.gameObject);

        allLines.RemoveAt(last);
    }
}
