using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KanjiJudge : MonoBehaviour
{
    Joycon joycon;

    [Header("参照（必須）")]
    public Image kanjiImage;              // UIの漢字Image
    public RectTransform kanjiRect;       // kanjiImageのRectTransform
    public JoyconDrawLine drawLine;       // GetAllPoints() を持つもの

    [Header("判定設定")]
    [Range(0.1f, 1.0f)]
    public float whiteThreshold = 0.5f;   // 白判定しきい値（0〜1）

    [Header("デバッグ")]
    public bool debugLog = false;

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
        if (joycon == null) return;

        // ※ここはあなたの「Bボタン割り当て」に合わせてください
        // いまBで遷移できているなら、同じボタンをここに設定
        if (joycon.GetButtonDown(Joycon.Button.DPAD_DOWN))
        {
            JudgeAndSendResult();
        }
    }

    public void JudgeAndSendResult()
    {
        if (drawLine == null || kanjiImage == null || kanjiRect == null)
        {
            Debug.LogError("KanjiJudge: 参照が設定されていません");
            return;
        }

        if (kanjiImage.sprite == null)
        {
            Debug.LogError("KanjiJudge: kanjiImage.sprite が空です");
            return;
        }

        Sprite sp = kanjiImage.sprite;
        Texture2D tex = sp.texture;

        if (!tex.isReadable)
        {
            Debug.LogError("KanjiJudge: Texture が Read/Write 無効です。Import Settingsで Read/Write Enabled をONにしてください。");
            return;
        }

        // 線の点を取得
        List<Vector3> pointsWorld = drawLine.GetAllPoints();
        if (pointsWorld == null || pointsWorld.Count == 0)
        {
            if (debugLog) Debug.Log("KanjiJudge: 判定点が0です（線がない）");
            GameManager.Instance.SetResult(0f);
            GameManager.Instance.ShowResult();
            return;
        }

        // ★ Canvasとカメラ（これが超重要）
        Canvas canvas = kanjiRect.GetComponentInParent<Canvas>();
        Camera uiCam = null;
        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            uiCam = canvas.worldCamera; // ★ Camera.main ではなく Canvas の camera
        }

        // SpriteがTextureのどこを使っているか（これが超重要）
        Rect tr = sp.textureRect; // ピクセル座標の切り出し領域
        // 例：atlasなら textureRect が一部分になる

        int total = 0;
        int hit = 0;

        for (int i = 0; i < pointsWorld.Count; i++)
        {
            // world → screen（Canvasカメラを使う）
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(uiCam, pointsWorld[i]);

            // screen → kanjiRect のローカル座標
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(kanjiRect, screenPos, uiCam, out Vector2 local))
                continue;

            // local → 0..1 のUV（Rect基準）
            Rect r = kanjiRect.rect;
            float u = (local.x - r.xMin) / r.width;
            float v = (local.y - r.yMin) / r.height;

            if (u < 0f || u > 1f || v < 0f || v > 1f)
                continue;

            total++;

            // ★ UV → SpriteのtextureRect にマッピング
            int px = Mathf.Clamp(Mathf.FloorToInt(tr.x + u * tr.width), 0, tex.width - 1);
            int py = Mathf.Clamp(Mathf.FloorToInt(tr.y + v * tr.height), 0, tex.height - 1);

            Color c = tex.GetPixel(px, py);

            if (debugLog && total <= 5)
            {
                Debug.Log($"local={local} uv=({u:F3},{v:F3}) pixel=({px},{py}) col={c} gray={c.grayscale:F3}");
            }

            if (c.grayscale >= whiteThreshold)
                hit++;
        }

        float accuracy = (total == 0) ? 0f : (hit / (float)total) * 100f;

        if (debugLog)
            Debug.Log($"KanjiJudge: hit={hit} total={total} accuracy={accuracy:F1}% pointsWorld={pointsWorld.Count}");

        // 結果を保存→リザルトへ
        GameManager.Instance.SetResult(accuracy);
        GameManager.Instance.ShowResult();
    }
}
