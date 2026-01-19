using UnityEngine;
using TMPro;

public class ResultView : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void OnEnable()
    {
        if (resultText == null) return;
        resultText.text = $"���m�x�F{GameManager.Instance.lastAccuracy:F1}%";
    }
}
