using UnityEngine;

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter
}

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public Season currentSeason;
    public Vector3 playerPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
