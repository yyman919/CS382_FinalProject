using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int SelectedLevel { get; private set; } = 1; // Default to Level 1

    private void Awake()
    {
        // Ensure only one instance of LevelManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }
    }

    public void SetLevelAndStartGame(int level)
    {
        SelectedLevel = level; // Set the selected level
        Debug.Log("Selected Level: " + SelectedLevel);
        SceneManager.LoadScene("Tetris"); // Replace with your game scene name
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartScene"); // Replace with your main menu scene name
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelectScene"); // Replace with your level selection scene name
    }
}
