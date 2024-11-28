using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public float maxHealth = 100.0f;
    public float currentHealth;
    public int gold = 0;
    public int experience = 0;
    public int level = 1; // Start at level 1
    private int experienceThreshold = 100;

    public Slider healthSlider;
    public Slider expSlider;
    public TMP_Text timerText;
    public TMP_Text goldExpText;
    public TMP_Text levelText;
    public TMP_Text killCounterText;
    public bool IsDead { get; private set; } = false;

    public float regenRate = 0.1f;
    public float moveSpeed = 5f;
    public float attackSpeedMultiplier = 1f;
    public float damageMultiplier = 1f;

    private float elapsedTime = 0f;
    private int killCount = 0;

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
        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateGoldExpText();
        UpdateExpBar();
        UpdateLevelText();
        StartCoroutine(HealthRegenCoroutine());
        UpdateKillCounter();
    }

    private void Update()
    {
        if (!IsDead)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    public void IncrementKillCount()
    {
        killCount++;
        UpdateKillCounter();
    }

    private void UpdateKillCounter()
    {
        if (killCounterText != null)
        {
            killCounterText.text = $"Kills: {killCount}";
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

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

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Player gained {amount} gold. Total gold: {gold}");
        UpdateGoldExpText();
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        UpdateGoldExpText();
        UpdateExpBar();

        if (experience >= experienceThreshold)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceThreshold;
        experienceThreshold = Mathf.RoundToInt(experienceThreshold * 1.1f);
        UpdateExpBar();
        UpdateLevelText();

        GameManager.Instance?.ShowLevelUpPanel();
    }

    public void UpgradeHealth()
    {
        maxHealth += 10;
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void UpgradeMoveSpeed()
    {
        moveSpeed += 0.5f;
        Debug.Log($"Move speed upgraded to {moveSpeed}");
    }

    public void UpgradeAttackSpeed()
    {
        attackSpeedMultiplier *= 0.9f;
    }

    public void UpgradeDamage()
    {
        damageMultiplier += 0.1f;
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
            healthSlider.gameObject.SetActive(currentHealth < maxHealth);
        }
    }

    private void UpdateGoldExpText()
    {
        if (goldExpText != null)
        {
            goldExpText.text = $"Gold: {gold}\nEXP: {experience}";
        }
    }

    private void UpdateExpBar()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = experienceThreshold;
            expSlider.value = experience;
        }
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = $"Level: {level}";
        }
    }

    private IEnumerator HealthRegenCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (currentHealth < maxHealth && !IsDead)
            {
                currentHealth += regenRate;
                currentHealth = Mathf.Min(currentHealth, maxHealth);
                UpdateHealthUI();
            }
        }
    }

    private void Die()
    {
        IsDead = true;
        GameManager.Instance?.ShowRestartPanel();
        gameObject.SetActive(false);
    }
}
