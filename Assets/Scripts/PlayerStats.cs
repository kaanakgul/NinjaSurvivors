using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerStats : MonoBehaviour
{
    public int gold = 0;           // Current gold
    public int experience = 0;     // Current experience points
    public int level = 0;          // Player's current level
    private int experienceThreshold = 500;  // EXP needed to reach the next level
    private const float levelIncrement = 1.1f;  // Increase threshold by 10% each level

    public TextMeshProUGUI goldExpText; // Reference to the UI Text

    private void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        CheckForLevelUp();
        UpdateUI();
    }

    private void CheckForLevelUp()
    {
        while (experience >= experienceThreshold)
        {
            experience -= experienceThreshold;  // Deduct the threshold amount from EXP
            LevelUp();  // Increase player level
            experienceThreshold = Mathf.RoundToInt(experienceThreshold * (1 + levelIncrement)); // Increase threshold by 10%
        }
    }

    private void LevelUp()
    {
        level++;  // Increment level
        Debug.Log("Leveled up to level " + level + "! New EXP threshold: " + experienceThreshold);
    }

    // Method to update UI with current stats, including level, gold, and experience
    private void UpdateUI()
    {
        if (goldExpText != null)
        {
            goldExpText.text = $"Gold: {gold}\nEXP: {experience}/{experienceThreshold}\nLevel: {level}";
        }
    }
}
