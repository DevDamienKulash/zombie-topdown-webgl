using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public static UnityEvent<int> onScoreChanged = new UnityEvent<int>();
    int score;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void Ensure()
    {
        if (!Instance)
        {
            var go = new GameObject("ScoreManager");
            go.AddComponent<ScoreManager>();
        }
    }

    public static void Reset()
    {
        Ensure();
        Instance.score = 0;
        onScoreChanged.Invoke(0);
        Debug.Log("[ScoreManager] Reset → 0");
    }

    public static void Add(int amount)
    {
        Ensure();
        Instance.score += amount;
        onScoreChanged.Invoke(Instance.score);
        Debug.Log($"[ScoreManager] Add {amount} → {Instance.score}");
    }

    public static int Current => Instance ? Instance.score : 0;
}
