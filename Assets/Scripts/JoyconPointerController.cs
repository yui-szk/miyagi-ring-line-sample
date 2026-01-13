using UnityEngine;

public class JoyconPointerController : MonoBehaviour
{
    Joycon joycon;

    [Header("感度（先端方向→画面座標）")]
    public float sensitivity = 8.0f;   // 大きいほど動く
    public float smooth = 12f;         // 大きいほど滑らか

    [Header("画面内に制限（ワールド座標）")]
    public Vector2 minLimit = new Vector2(-8f, -4.5f);
    public Vector2 maxLimit = new Vector2(8f, 4.5f);

    [Header("縦持ち補正")]
    public bool invertX = false;
    public bool invertY = true;        // 縦持ちだと上下が逆になりやすいので初期true推奨

    Quaternion baseRot = Quaternion.identity;   // Rで基準
    Vector3 velocity = Vector3.zero;
    Vector3 startPos;

    void Start()
    {
        if (JoyconManager.Instance == null || JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("Joy-Con が見つかりません");
            enabled = false;
            return;
        }

        joycon = JoyconManager.Instance.j[0];
        startPos = transform.position;

        // 最初に1回センターを取っておく
        baseRot = joycon.GetVector();
    }

    void Update()
    {
        if (joycon == null) return;

        // R（SHOULDER_1）でセンター（基準姿勢）を再取得＆ポインタ中央へ
        if (joycon.GetButtonDown(Joycon.Button.SHOULDER_1))
        {
            baseRot = joycon.GetVector();
            transform.position = startPos;
            velocity = Vector3.zero;
            return;
        }

        // 現在の姿勢
        Quaternion rot = joycon.GetVector();

        // 基準姿勢からの相対回転（Rでセンター）
        Quaternion rel = Quaternion.Inverse(baseRot) * rot;

        // Joy-Conの「先端方向」を決める
        // ここはライブラリ/持ち方で軸がズレるので調整ポイント
        // rel * Vector3.forward を基本に、合わなければ up/right を試す
        Vector3 dir = rel * Vector3.forward;

        // 画面平面への投影：x,y成分を使って座標にする
        float x = dir.x * sensitivity;
        float y = dir.y * sensitivity;

        if (invertX) x *= -1f;
        if (invertY) y *= -1f;

        Vector3 targetPos = new Vector3(startPos.x + x, startPos.y + y, 0f);

        // 画面外に出ないよう制限
        targetPos.x = Mathf.Clamp(targetPos.x, minLimit.x, maxLimit.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minLimit.y, maxLimit.y);

        // スムーズ移動
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            1f / smooth
        );
    }
}
