using UnityEngine;
using TMPro;

public class ResultScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText.text = ScoreManager.FinalScore.ToString();
    }
}