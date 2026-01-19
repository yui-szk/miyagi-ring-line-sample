using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameAppearance : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NAME;
    [SerializeField] private GameObject AppearanceNameWindow;

    public float displayDelay = 2.0f;
    private float timer;
    private bool isDisplayed;

    void Start()
    {
        Debug.Log("Start called");
        timer = 0.0f;
        AppearanceNameWindow.SetActive(false);
        NAME.gameObject.SetActive(false);
        isDisplayed = false;
    }

    void Update()
    {
        if (!isDisplayed)
        {
            timer += Time.deltaTime;
            Debug.Log("Timer: " + timer);

            if (timer >= displayDelay)
            {
                Debug.Log("Displaying NameWindow now.");
                AppearanceNameWindow.SetActive(true);
                NAME.gameObject.SetActive(true);
            }
        }
    }
}
