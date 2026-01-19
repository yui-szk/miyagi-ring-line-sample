using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWindowAppearance : MonoBehaviour
{
    public GameObject AppearanceTextWindow;
    public GameObject AppearanceNameWindow;

    [SerializeField] private TextMeshProUGUI NAME;
    public float displayDelay = 2.0f;
    private float timer;
    private bool isDisplayed;

    void Start()
    {
        Debug.Log("Start called");
        timer = 0.0f;
        isDisplayed = false;
        AppearanceTextWindow.SetActive(false);
        AppearanceNameWindow.SetActive(false);
        NAME.gameObject.SetActive(false);
    }

    private void Update()
    {
        
        if (!isDisplayed)
        {
            timer += Time.deltaTime;
            Debug.Log("Timer: " + timer);

            if (timer >= displayDelay)
            {
                Debug.Log("Displaying window now.");
                AppearanceTextWindow.SetActive(true);
                AppearanceNameWindow.SetActive(true);
                NAME.gameObject.SetActive(true);
                isDisplayed = true;
            }
        }
    }
}
