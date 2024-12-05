using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string upgradeDescription; // Description of the upgrade
    public string upgradeType; // Type of upgrade (e.g., "Health", "MoveSpeed", etc.)
    public string weaponName; // Weapon name (if this is a weapon upgrade)
    public int maxLevel = 0; // Max level for the upgrade
    public int cost; // Cost for the upgrade

    private LevelUpManager levelUpManager;

    private void Start()
    {
        levelUpManager = LevelUpManager.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        int currentLevel = GetCurrentLevel();
        string tooltipContent = $"{upgradeDescription}";

        if (maxLevel > 0)
        {
            tooltipContent = $"Level: {currentLevel}/{maxLevel}\n{tooltipContent}";
        }
        else
        {
            tooltipContent = $"Level: {currentLevel}\n{tooltipContent}";
        }

        if (cost > 0)
        {
            tooltipContent += $"\nCost: {cost} gold";
        }

        if (!string.IsNullOrEmpty(weaponName) && WeaponManager.Instance != null)
        {
            int cost = WeaponManager.Instance.GetUpgradeCost(weaponName);
            tooltipContent += $"\nCost: {cost} gold";
        }

        levelUpManager?.ShowTooltip(tooltipContent, Input.mousePosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        levelUpManager?.HideTooltip();
    }

    public void OnButtonClick()
    {
        // Ensure the tooltip is hidden when a button is clicked
        levelUpManager?.HideTooltip();

        // Add logic for the button click, if needed (e.g., trigger upgrade)
    }

    private int GetCurrentLevel()
    {
        // Check if this is a weapon upgrade
        if (!string.IsNullOrEmpty(weaponName) && WeaponManager.Instance != null)
        {
            return WeaponManager.Instance.GetWeaponLevel(weaponName);
        }

        // Check if this is a stat upgrade
        if (!string.IsNullOrEmpty(upgradeType) && PlayerStats.Instance != null)
        {
            return GetStatLevel(upgradeType);
        }

        return 0; // Default level if no valid source
    }

    private int GetStatLevel(string statType)
    {
        switch (statType)
        {
            case "Health": return PlayerStats.Instance.healthLevel;
            case "MoveSpeed": return PlayerStats.Instance.moveSpeedLevel;
            case "AttackSpeed": return PlayerStats.Instance.attackSpeedLevel;
            case "Damage": return PlayerStats.Instance.damageLevel;
            case "Lifesteal": return PlayerStats.Instance.lifestealLevel;
            case "Regen": return PlayerStats.Instance.regenLevel;
            default: return 0; // Default if invalid statType
        }
    }
}