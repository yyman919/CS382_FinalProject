using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisGameManager : MonoBehaviour
{
    public void QuitToGameOver()
    {
        // Stop the background music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBackgroundMusic();
        }

        // Load the Game Over scene
        SceneManager.LoadScene("GameOver"); // Replace with your Game Over scene name
    }
}
