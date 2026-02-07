using UnityEngine;
using TMPro;

public class ResultView : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI accuracyText; // ％表示
    public TextMeshProUGUI rankText;     // ランク表示
    public TextMeshProUGUI rankText1;    // ランク表示

    void OnEnable()
    {
        float acc = GameManager.Instance.lastAccuracy;

        if (accuracyText != null)
            accuracyText.text = $"{acc:F1}%";

        if (rankText != null)
            rankText.text = GetRank(acc);
      //  if (rankText1 != null)
        //    rankText1.text = GetRank(acc);
    }

    string GetRank(float acc)
    {
        if (acc >= 90f) return "S";
        if (acc >= 80f) return "A";
        if (acc >= 70f) return "B";
        if (acc >= 50f) return "C";
        return "D";
    }
} // ← クラスの閉じ括弧を追加
