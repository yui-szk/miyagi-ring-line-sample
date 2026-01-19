using UnityEngine;

public class SakuraResultImage : MonoBehaviour
{
    [Header("Clear Settings")]
    public int clearBorder = 20;   // Clear threshold (number of cherry blossoms)

    [Header("Display Images")]
    public GameObject goodImage;   // Success Image
    public GameObject badImage;    // Failure Image

    void Start()
    {
        // Initial Deactivation
        goodImage.SetActive(false);
        badImage.SetActive(false);

        // Assuming GameManager2 exists and has FinalScore
        int score = GameManager2.FinalScore;

        Debug.Log("Final Sakura Count = " + score);

        if (score >= clearBorder)
        {
            goodImage.SetActive(true);
        }
        else
        {
            badImage.SetActive(true);
        }
    }
}
