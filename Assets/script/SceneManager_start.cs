using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class JoyconSceneChange : MonoBehaviour
{
    Joycon joycon;
    bool isChanging = false;

    [SerializeField] string nextSceneName = "NextScene";

    [Header("SE設定")]
    [SerializeField] AudioClip buttonSE;

    [Header("タイミング調整（Inspectorで変更）")]
    [SerializeField] float seDelay = 0.2f;   // SE鳴らしてから待つ時間
    [SerializeField] float extraDelay = 0f;   // 追加の余裕（任意）

    void Start()
    {
        if (JoyconManager.Instance == null || JoyconManager.Instance.j.Count == 0)
        {
            Debug.LogError("Joyconが見つかりません");
            return;
        }

        joycon = JoyconManager.Instance.j[0];
    }

    void Update()
    {
        if (joycon == null) return;

        if (!isChanging && joycon.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            isChanging = true;
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        // SE再生
        if (SEManager.Instance != null && buttonSE != null)
        {
            SEManager.Instance.PlaySE(buttonSE);
        }

        // ★Inspectorで調整できる待ち時間
        yield return new WaitForSeconds(seDelay + extraDelay);

        SceneManager.LoadScene(nextSceneName);
    }
}