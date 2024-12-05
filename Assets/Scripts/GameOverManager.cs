using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("StartScene"); // Replace with your Main Menu scene name
    }

}
