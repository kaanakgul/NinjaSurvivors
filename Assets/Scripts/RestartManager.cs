using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    public GameObject restartPanel; // Reference to the restart panel
    private PlayerStats playerStats; // Reference to PlayerStats

    private void Start()
    {
        // Ensure the restart panel is hidden at the start
        restartPanel.SetActive(false);
        playerStats = FindObjectOfType<PlayerStats>(); // Get the PlayerStats instance
    }

    private void Update()
    {
        // Check if playerStats is null
        if (playerStats == null)
        {
            Debug.LogWarning("PlayerStats not found!"); // Log warning if not found
            return; // Exit Update if playerStats is not found
        }

        // Check if the player is dead
        if (playerStats.IsDead)
        {
            ShowRestartPanel();
        }
        else
        {
            restartPanel.SetActive(false); // Hide the restart panel if the player is not dead
        }

        // Listen for restart input
        if (Input.GetKeyDown(KeyCode.R) && playerStats.IsDead)
        {
            RestartGame();
        }
    }

    private void ShowRestartPanel()
    {
        restartPanel.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }
}
