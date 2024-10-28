using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public int gold = 0;
    public int experience = 0;

    public Slider healthSlider; // Reference to health bar UI

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
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

    // Check if the player is dead
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // Method to handle player death
    private void Die()
    {
        Debug.Log("Player is dead.");
        // Add any additional death handling here, e.g., disable player controls
        gameObject.SetActive(false); // Deactivate player on death
    }

    // Method to add gold
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Player gained " + amount + " gold. Total gold: " + gold);
    }

    // Method to add experience
    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log("Player gained " + amount + " experience. Total experience: " + experience);
        // Handle leveling up here if necessary
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
}
