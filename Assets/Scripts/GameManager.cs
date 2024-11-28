using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject restartPanel; // Restart panel UI
    public GameObject levelUpPanel; // Level-up panel UI
    private bool isGameOver = false; // Track if the game is over

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ensure panels are hidden at the start
        if (restartPanel != null)
            restartPanel.SetActive(false);

        if (levelUpPanel != null)
            levelUpPanel.SetActive(false);
    }

    private void Update()
    {
        // Check for restart input if the game is over
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    /// <summary>
    /// Pause the game.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Paused.");
    }

    /// <summary>
    /// Resume the game.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game Resumed.");
    }

    /// <summary>
    /// Show the restart panel and pause the game.
    /// </summary>
    public void ShowRestartPanel()
    {
        if (restartPanel != null)
        {
            isGameOver = true;
            restartPanel.SetActive(true);
            PauseGame();
        }
    }

    /// <summary>
    /// Restart the current scene.
    /// </summary>
    public void RestartGame()
    {
        isGameOver = false; // Reset game-over status
        ResumeGame(); // Ensure time scale is reset
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game Restarted.");
    }


    /// <summary>
    /// Show the level-up panel.
    /// </summary>
    public void ShowLevelUpPanel()
    {
        if (levelUpPanel != null)
        {
            if (!levelUpPanel.activeSelf) // Ensure the panel isn't already active
            {
                levelUpPanel.SetActive(true);
                Debug.Log("Level-Up Panel displayed.");
                PauseGame(); // Pause the game only when displaying the panel
            }
            else
            {
                Debug.LogWarning("Level-Up Panel is already active!");
            }
        }
        else
        {
            Debug.LogError("Level-Up Panel is not assigned in GameManager!");
        }
    }


    /// <summary>
    /// Close the level-up panel.
    /// </summary>
    public void CloseLevelUpPanel()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
            ResumeGame();
        }
    }
}
