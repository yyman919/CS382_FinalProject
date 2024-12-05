using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Tetris"); // Replace with your actual game scene name
    }

    public void ShowRules()
    {
        SceneManager.LoadScene("RulesScene"); // Replace with your Rules scene name
    }

    public void SelectLevel()
    {
        SceneManager.LoadScene("LevelSelectScene"); // Replace with your Level Select scene name
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("StartScene"); // Replace with your Start Scene name
    }
}
