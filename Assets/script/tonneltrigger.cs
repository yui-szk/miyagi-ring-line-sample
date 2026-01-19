using UnityEngine;

public class TunnelTrigger : MonoBehaviour
{
    public string nextSceneName;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Train"))
        {
            FadeManager.Instance.FadeAndLoadScene(
                nextSceneName,
                1f, // フェードアウト
                2.5f, // 暗闇
                1f  // フェードイン
            );
        }
    }
}
