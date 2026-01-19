using UnityEngine;

public class JoyconBootstrap : MonoBehaviour
{
    static JoyconBootstrap instance;

    void Awake()
    {
        Debug.Log("🔥 JoyconBootstrap Awake CALLED");

        if (instance != null)
        {
            Debug.Log("❌ Duplicate JoyconBootstrap destroyed");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("✅ JoyconBootstrap Start CALLED");
    }
}
