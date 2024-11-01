using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public int gold = 0;
    public int experience = 0;
    public int level = 1; // Start at level 1
    private int experienceThreshold = 100; // Initial EXP needed for the next level
    public Slider healthSlider; // Reference to health bar UI
    public Slider expSlider; // Reference to EXP bar UI
    public TMP_Text timerText; // Reference to the timer TextMeshPro component
    public TMP_Text goldExpText; // Reference to the UI TextMeshPro component for gold and experience
    public TMP_Text levelText; // Reference to the level indicator TextMeshPro component
    public TMP_Text killCounterText; // Reference to the kill counter TextMeshPro component
    public bool IsDead { get; private set; } = false; // Property for dead state

    public float regenRate = 0.1f; // Health per second for regeneration
    public WeaponManager weaponManager; // Reference to WeaponManager for shuriken upgrades
    private float elapsedTime = 0f; // Timer to track elapsed game time
    private int killCount = 0; // Count of enemies killed

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateGoldExpText();
        UpdateExpBar();
        UpdateLevelText(); // Update level text at the start to reflect correct level
        StartCoroutine(HealthRegenCoroutine());
        UpdateKillCounter();
    }

    // Method to take damage
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        UpdateHealthUI();
    }

    private void Update()
    {
        // Update the timer
        if (!IsDead)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            elapsedTime = 0f;
            UpdateTimerText();
        }
    }

    // Method to increment kill count and update UI
    public void IncrementKillCount()
    {
        killCount++;
        UpdateKillCounter();
    }

    private void UpdateKillCounter()
    {
        if (killCounterText != null) // Ensure this is assigned in the inspector
        {
            killCounterText.text = $"Kills: {killCount}";
        }
        else
        {
            Debug.LogWarning("Kill Counter TextMeshPro object is not assigned!");
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null) // Ensure this is assigned in the inspector
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
        else
        {
            Debug.LogWarning("Timer TextMeshPro object is not assigned!");
        }
    }

    // Method to handle player death
    private void Die()
    {
        IsDead = true; // Set the player as dead
        Debug.Log("Player is dead.");
        gameObject.SetActive(false); // Deactivate player on death
    }

    // Method to add gold
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Player gained " + amount + " gold. Total gold: " + gold);
        UpdateGoldExpText();
    }

    // Method to add experience
    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log("Player gained " + amount + " experience. Total experience: " + experience);
        UpdateGoldExpText();
        UpdateExpBar();

        // Check if the player has leveled up
        if (experience >= experienceThreshold)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceThreshold; // Reset current experience after leveling up
        experienceThreshold = Mathf.RoundToInt(experienceThreshold * 1.1f); // Increase threshold for next level
        Debug.Log("Leveled up! New level: " + level);
        UpdateExpBar();
        UpdateLevelText(); // Update level text after leveling up

        // Upgrade weapons at certain levels
        if (weaponManager != null)
        {
            weaponManager.UpgradeShuriken(level);
        }
    }

    // Method to update the EXP bar UI
    private void UpdateExpBar()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = experienceThreshold;
            expSlider.value = experience;
        }
    }

    // Health regeneration coroutine
    private IEnumerator HealthRegenCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Check every second

            // Regenerate health if below max and not dead
            if (currentHealth < maxHealth && !IsDead)
            {
                currentHealth += regenRate;
                currentHealth = Mathf.Min(currentHealth, maxHealth); // Clamp to max health
                UpdateHealthUI();
            }
        }
    }

    // Method to update the health bar UI
    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
            healthSlider.gameObject.SetActive(currentHealth < maxHealth); // Show only if health is not full
        }
    }

    // Method to update the gold and experience text UI
    public void UpdateGoldExpText()
    {
        if (goldExpText != null) // Ensure this is assigned in the inspector
        {
            goldExpText.text = $"Gold: {gold}\nEXP: {experience}";
        }
        else
        {
            Debug.LogWarning("Gold/EXP TextMeshPro object is not assigned!");
        }
    }

    // Method to update the level text UI
    public void UpdateLevelText()
    {
        if (levelText != null) // Ensure this is assigned in the inspector
        {
            levelText.text = $"Level: {level}";
        }
        else
        {
            Debug.LogWarning("Level TextMeshPro object is not assigned!");
        }
    }
}
