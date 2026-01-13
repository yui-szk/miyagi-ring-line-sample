using UnityEngine;
using TMPro;

public class ResultView : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void OnEnable()
    {
        if (resultText == null) return;
        resultText.text = $"ê≥ämìxÅF{GameManager.Instance.lastAccuracy:F1}%";
    }
}
