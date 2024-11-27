using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpScreen; // The main level-up screen
    public GameObject statUpgradeScreen; // The stat upgrade screen
    public GameObject weaponUpgradeScreen; // The weapon upgrade screen

    public static LevelUpManager Instance { get; private set; }
    private bool isUpgrading = false; // Prevent multiple upgrades at once

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

    /// <summary>
    /// Trigger the level-up process and display the level-up menu.
    /// </summary>
    public void TriggerLevelUp(int level)
    {
        if (isUpgrading) return;

        Debug.Log($"Level {level} reached. Triggering level-up UI.");
        GameManager.Instance?.PauseGame(); // Pause the game using GameManager
        levelUpScreen.SetActive(true); // Show the level-up screen
        isUpgrading = true; // Prevent multiple level-ups at the same time
    }

    /// <summary>
    /// Open the stat upgrade screen.
    /// </summary>
    public void ChooseStatUpgrade()
    {
        if (!isUpgrading) return;

        Debug.Log("Stat upgrade option selected.");
        levelUpScreen.SetActive(false); // Hide the main level-up screen
        statUpgradeScreen.SetActive(true); // Show the stat upgrade screen
    }

    /// <summary>
    /// Open the weapon upgrade screen.
    /// </summary>
    public void ChooseWeaponUpgrade()
    {
        if (!isUpgrading) return;

        Debug.Log("Weapon upgrade option selected.");
        levelUpScreen.SetActive(false); // Hide the main level-up screen
        weaponUpgradeScreen.SetActive(true); // Show the weapon upgrade screen
    }

    /// <summary>
    /// Apply a stat upgrade based on the selected stat type.
    /// </summary>
    public void UpgradeStat(string statType)
    {
        Debug.Log($"UpgradeStat button pressed with parameter: {statType}");

        if (!isUpgrading || PlayerStats.Instance == null)
        {
            Debug.LogWarning("UpgradeStat failed: either not upgrading or PlayerStats instance is null.");
            return;
        }

        switch (statType)
        {
            case "Health":
                PlayerStats.Instance.UpgradeHealth();
                break;
            case "MoveSpeed":
                PlayerStats.Instance.UpgradeMoveSpeed();
                break;
            case "AttackSpeed":
                PlayerStats.Instance.UpgradeAttackSpeed();
                break;
            case "Damage":
                PlayerStats.Instance.UpgradeDamage();
                break;
            default:
                Debug.LogWarning($"Invalid stat type '{statType}' selected for upgrade.");
                return;
        }

        Debug.Log($"Stat '{statType}' upgraded successfully.");
        CloseUpgradeScreens();
    }

    /// <summary>
    /// Apply a weapon upgrade based on the selected weapon name.
    /// </summary>
    public void UpgradeWeapon(string weaponName)
    {
        Debug.Log($"UpgradeWeapon button pressed with parameter: {weaponName}");

        if (!isUpgrading || WeaponManager.Instance == null)
        {
            Debug.LogWarning("UpgradeWeapon failed: either not upgrading or WeaponManager instance is null.");
            return;
        }

        WeaponManager.Instance.UpgradeWeapon(weaponName);
        Debug.Log($"Weapon '{weaponName}' upgraded successfully.");
        CloseUpgradeScreens();
    }

    /// <summary>
    /// Close all upgrade screens and resume the game.
    /// </summary>
    private void CloseUpgradeScreens()
    {
        levelUpScreen.SetActive(false);
        statUpgradeScreen.SetActive(false);
        weaponUpgradeScreen.SetActive(false);

        GameManager.Instance?.ResumeGame(); // Resume the game using GameManager
        isUpgrading = false; // Allow future upgrades
    }
}
