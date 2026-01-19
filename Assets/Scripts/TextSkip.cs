using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSkip : MonoBehaviour
{
    [SerializeField] private GameObject textWindow;
    [SerializeField] private GameObject Click;
    [SerializeField] private TextMeshProUGUI textUI;

    [TextArea(4, 4)]
    [SerializeField] private string[] sentences;

    [SerializeField] private float TextAppearance = 1.5f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private int Index = 0;
    private bool isReady = false;

    void Start()
    {
        textUI.text = "";
        StartCoroutine(FirstText());
        Click.SetActive(false);
    }

    private IEnumerator FirstText()
    {
        yield return new WaitForSeconds(TextAppearance);
        ShowSentence();
        isReady = true;
    }

    private void ShowSentence()
    {
        //textUI.text = sentences[Index];

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentence(sentences[Index]));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textUI.text = "";

        foreach (char c in sentence)
        {
            textUI.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
        typingCoroutine = null;

        if (Click != null)
            Click.SetActive(true);
    }

    void Update()
    {
        if (!isReady) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                textUI.text = sentences[Index];
                isTyping = false;

                if (Click != null)
                    Click.SetActive(true);
                return;
            }

                Index++;

            if (Index < sentences.Length)
            {
                if (Click != null)
                    Click.SetActive(false);
                ShowSentence();
                //textUI.text = sentences[Index];
            }
            else
            {
                Debug.Log("会話終了");
                textUI.gameObject.SetActive(false);
                if (Click != null)
                    Click.SetActive(false);
                FadeBlack fade = FindObjectOfType<FadeBlack>();
                fade.FadeIn_SceneEnd();
            }
        }
    }
}
