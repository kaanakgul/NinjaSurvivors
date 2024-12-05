using UnityEngine;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpScreen; // The main level-up screen
    public GameObject statUpgradeScreen; // The stat upgrade screen
    public GameObject weaponUpgradeScreen; // The weapon upgrade screen
    public GameObject tooltipObject; // Tooltip UI panel
    public TMP_Text tooltipText; // TMP Text for the tooltip

    public static LevelUpManager Instance; // Singleton for easy access

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
        tooltipObject.SetActive(false); // Ensure the tooltip is hidden initially
    }


    // Trigger the level-up process and display the level-up menu
    public void TriggerLevelUp(int level)
    {
        Debug.Log($"Level {level} reached. Triggering level-up UI.");
        levelUpScreen.SetActive(true); // Show the level-up screen
        GameManager.Instance?.PauseGame(); // Pause the game using GameManager
        Debug.Log("Level-Up process started.");
    }

    // Choose to upgrade a stat and display the stat upgrade screen
    public void ChooseStatUpgrade()
    {
        Debug.Log("ChooseStatUpgrade called.");
        levelUpScreen.SetActive(false); // Hide the main level-up screen
        statUpgradeScreen.SetActive(true); // Show the stat upgrade screen
        Debug.Log("Stat upgrade screen displayed.");
    }

    // Choose to upgrade a weapon and display the weapon upgrade screen
    public void ChooseWeaponUpgrade()
    {
        Debug.Log("ChooseWeaponUpgrade called.");
        levelUpScreen.SetActive(false); // Hide the main level-up screen
        weaponUpgradeScreen.SetActive(true); // Show the weapon upgrade screen
        Debug.Log("Weapon upgrade screen displayed.");
    }

    public void ShowTooltip(string content, Vector3 position)
    {
        tooltipObject.SetActive(true);
        tooltipText.text = content;

        // Convert screen position to local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tooltipObject.transform.parent.GetComponent<RectTransform>(), // Parent RectTransform (Canvas)
            position, // Mouse position
            Camera.main, // The camera rendering the Canvas
            out Vector2 localPoint // Output local position
        );

        tooltipObject.GetComponent<RectTransform>().localPosition = localPoint; // Set the tooltip's position
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }


    // Apply a stat upgrade based on the selected stat type
    public void UpgradeStat(string statType)
    {
        if (PlayerStats.Instance == null) return;

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
            case "Lifesteal":
                PlayerStats.Instance.UpgradeLifesteal();
                break;
            case "Regen":
                PlayerStats.Instance.UpradeRegen();
                break;
            default:
                Debug.LogWarning($"Invalid stat type '{statType}' selected for upgrade.");
                return;
        }

        Debug.Log($"Stat '{statType}' upgraded successfully.");
        CloseUpgradeScreens();
    }

    // Apply a weapon upgrade based on the selected weapon name
    public void UpgradeWeapon(string weaponName)
    {
        if (WeaponManager.Instance == null) return;

        // Check if the upgrade was successful
        bool upgradeSuccessful = WeaponManager.Instance.UpgradeWeapon(weaponName);

        if (upgradeSuccessful)
        {
            Debug.Log($"Weapon '{weaponName}' upgraded successfully.");
            CloseUpgradeScreens(); // Close the menu only if the upgrade was successful
        }
        else
        {
            Debug.Log($"Weapon '{weaponName}' is already at max level.");
            // The menu remains open, and the player can select another upgrade
        }
    }

    public void ShowWeaponUpgradeWarning(string message)
    {
        Debug.Log(message);
        // Additional UI logic to display the warning in-game can be added here
    }

    // Close all upgrade screens and resume the game
    private void CloseUpgradeScreens()
    {
        Debug.Log("Closing upgrade screens...");
        levelUpScreen.SetActive(false);
        statUpgradeScreen.SetActive(false);
        weaponUpgradeScreen.SetActive(false);

        GameManager.Instance.ResumeGame(); // Resume the game using GameManager
        Debug.Log("Upgrade process complete.");
    }

    public void GoBackToLevelUp()
    {
        Debug.Log("Returning to Level-Up menu.");

        // Hide all upgrade screens
        statUpgradeScreen.SetActive(false);
        weaponUpgradeScreen.SetActive(false);

        // Show the Level-Up screen
        levelUpScreen.SetActive(true);
    }

}
