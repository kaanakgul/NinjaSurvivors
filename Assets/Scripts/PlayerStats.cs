using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int gold = 0;         // Current gold
    public int experience = 0;   // Current experience points

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
        UpdateUI();
    }

    // Method to update UI with current gold and experience values
    private void UpdateUI()
    {
        if (goldExpText != null)
        {
            goldExpText.text = $"Gold: {gold}\nEXP: {experience}";
        }
    }
}
