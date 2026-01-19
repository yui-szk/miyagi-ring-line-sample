using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class IntroSceneManager : MonoBehaviour
{
    // --- Inspectorで設定するオブジェクト ---
    public GameObject trainDoorLeft;      // 左ドアのキューブ
    public GameObject trainDoorRight;     // 右ドアのキューブ
    public TextMeshProUGUI explanationText; // 説明テキストUI
    public Camera mainCamera;            // ★ 新規: 動かすメインカメラ

    public string mainGameSceneName = "MainGameScene";

    // --- ドア/カメラ設定 ---
    public float initialDelay = 10.0f;       // 演出開始までの待機時間
    public float doorAnimationDuration = 1.0f; // ドアが開くアニメーションの時間
    public float doorSlideDistance = 1.5f;   // ドアが左右にスライドする距離

    // ★ カメラ移動設定 ★
    public float cameraMoveDistance = 1.0f; // 前方に移動する距離
    public float cameraMoveDuration = 1.5f; // カメラ移動にかける時間

    // 説明表示後に待機する時間
    public float autoStartDelay = 3.0f;

    void Start()
    {
        // UIを非表示に設定
        if (explanationText != null)
        {
            explanationText.gameObject.SetActive(false);
        }

        // 演出シーケンスを開始
        StartCoroutine(OpeningSequence(initialDelay));
    }

    IEnumerator OpeningSequence(float delay)
    {
        // 1. 指定された時間だけ待機 (10秒)
        yield return new WaitForSeconds(delay);

        // 2. ドア開閉アニメーションの開始
        StartCoroutine(AnimateDoorOpen(doorAnimationDuration));

        // 3. ドアが開くのを待つ
        yield return new WaitForSeconds(doorAnimationDuration);

        // 4. ★ カメラ移動の開始 ★
        StartCoroutine(AnimateCameraMove(cameraMoveDuration));

        // 5. カメラ移動が終わるのを待つ
        yield return new WaitForSeconds(cameraMoveDuration);

        // 6. 説明を表示
        if (explanationText != null)
        {
            explanationText.gameObject.SetActive(true);
        }

        // 7. 自動遷移のための待機
        yield return new WaitForSeconds(autoStartDelay);

        // 8. ゲーム開始
        StartGame();
    }

    IEnumerator AnimateDoorOpen(float duration)
    {
        if (trainDoorLeft == null || trainDoorRight == null) yield break;

        float startTime = Time.time;

        Vector3 initialPosL = trainDoorLeft.transform.localPosition;
        Vector3 initialPosR = trainDoorRight.transform.localPosition;

        Vector3 finalPosL = initialPosL + Vector3.left * doorSlideDistance;
        Vector3 finalPosR = initialPosR + Vector3.right * doorSlideDistance;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            trainDoorLeft.transform.localPosition = Vector3.Lerp(initialPosL, finalPosL, t);
            trainDoorRight.transform.localPosition = Vector3.Lerp(initialPosR, finalPosR, t);
            yield return null;
        }

        trainDoorLeft.transform.localPosition = finalPosL;
        trainDoorRight.transform.localPosition = finalPosR;
    }

    // ★ 新規: カメラを前方に移動させるコルーチン ★
    IEnumerator AnimateCameraMove(float duration)
    {
        if (mainCamera == null) yield break;

        float startTime = Time.time;
        Vector3 initialPos = mainCamera.transform.position;

        // Z軸の負の方向（カメラの正面方向）へ移動
        Vector3 finalPos = initialPos + mainCamera.transform.forward * cameraMoveDistance;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            mainCamera.transform.position = Vector3.Lerp(initialPos, finalPos, t);
            yield return null;
        }

        mainCamera.transform.position = finalPos;
    }

    // ゲーム開始メソッド（自動で呼び出される）
    public void StartGame()
    {
        SceneManager.LoadScene(mainGameSceneName);
    }
}