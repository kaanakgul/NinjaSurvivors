using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // Replace with your game scene name.
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings"); // Replace with your game scene name.
        Debug.Log("Open Settings Menu"); // Add functionality later.
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit"); // Logs quit in editor (doesn't work in builds).
    }
}
