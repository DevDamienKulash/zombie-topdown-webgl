using UnityEngine;
using TMPro;

public class ScoreHUD_TMP : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;

    void Awake()
    {
        if (!scoreText) scoreText = GetComponent<TMP_Text>();
        ScoreManager.Ensure(); // guarantees an instance exists
    }

    void OnEnable()
    {
        ScoreManager.onScoreChanged.AddListener(UpdateScore);
        UpdateScore(ScoreManager.Current); // immediate refresh
    }

    void OnDisable()
    {
        ScoreManager.onScoreChanged.RemoveListener(UpdateScore);
    }

    void Update()  // fallback — keeps UI in sync even if event missed
    {
        if (scoreText) scoreText.text = ScoreManager.Current.ToString();
    }

    void UpdateScore(int value)
    {
        if (scoreText) scoreText.text = value.ToString();
    }
}
