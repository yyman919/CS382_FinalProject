using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText;  // Assign in Inspector
    private int score;
    private int comboCount;  // Track consecutive line clears

    private void Awake()
    {
        // Singleton pattern to ensure only one ScoreManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional if you want to keep score across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetScore();  // Ensure the score starts at zero
    }

    public void AddScore(int linesCleared)
{
    int pointsEarned = linesCleared * (100 + 50 * comboCount * comboCount);
    score += pointsEarned;
    comboCount++;
    UpdateScoreText();
}


    public void ResetCombo()
    {
        comboCount = 0;  // Reset combo count when the combo streak ends
    }

    public void ResetScore()
    {
        score = 0;
        comboCount = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
}
