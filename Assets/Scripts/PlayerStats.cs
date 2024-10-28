using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include TextMeshPro namespace

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public int gold = 0;
    public int experience = 0;

    public Slider healthSlider; // Reference to health bar UI
    public TMP_Text goldExpText; // Reference to the UI TextMeshPro component for gold and experience
    public bool IsDead { get; private set; } = false; // Property for dead state

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateGoldExpText(); // Initialize the text display
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
        UpdateGoldExpText(); // Update the UI whenever gold is added
    }

    // Method to add experience
    public void AddExperience(int amount)
    {
        experience += amount;
        Debug.Log("Player gained " + amount + " experience. Total experience: " + experience);
        UpdateGoldExpText(); // Update the UI whenever experience is added
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
}
