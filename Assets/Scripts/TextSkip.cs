using System.Collections;
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
    [SerializeField] private string nextSceneName = "KanjiGame";

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private int Index = 0;
    private bool isReady = false;

    // ===== Joy-Con =====
    Joycon joycon;
    bool lastAState = false;

    void Awake()
    {
        if (JoyconManager.Instance != null &&
            JoyconManager.Instance.j.Count > 0)
        {
            joycon = JoyconManager.Instance.j[0];
            Debug.Log("✅ Joycon assigned to TextSkip");
        }
        else
        {
            Debug.LogError("❌ Joycon not found in TextSkip");
        }
    }

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
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

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
        Click.SetActive(true);
    }

    void Update()
    {
        if (!isReady) return;

        if (IsJoyconAPressed())
        {
            HandleNext();
        }
    }

    // ===== Joy-Con Aボタン（高速ON/OFF対策済み）=====
    bool IsJoyconAPressed()
    {
        if (joycon == null) return false;

        // JoyconLibでは A = DPAD_RIGHT
        bool current = joycon.GetButton(Joycon.Button.DPAD_RIGHT);

        bool pressed = current && !lastAState;
        lastAState = current;

        return pressed;
    }

    void HandleNext()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            textUI.text = sentences[Index];
            isTyping = false;
            Click.SetActive(true);
            return;
        }

        Index++;

        if (Index < sentences.Length)
        {
            Click.SetActive(false);
            ShowSentence();
        }
        else
        {
            Debug.Log("会話終了");

            FadeBlack fade = FindObjectOfType<FadeBlack>();
            fade.FadeIn_SceneEnd(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
            });
        }
    }
}
