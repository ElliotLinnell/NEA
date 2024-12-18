using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthRegenRate = 2f;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 5f;
    public float staminaConsumptionRate = 20f;
    public float staminaRegenDelay = 2f;

    [Header("UI Settings")]
    public Slider healthBar;
    public Slider staminaBar;

    private float lastStaminaUseTime;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = currentStamina;
        }
    }

    void Update()
    {
        RegenerateHealth();
        RegenerateStamina();
        UpdateUI();
    }

    private void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }

    private void RegenerateStamina()
    {
        if (Time.time >= lastStaminaUseTime + staminaRegenDelay)
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            }
        }
    }

    private void UpdateUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (staminaBar != null)
        {
            staminaBar.value = currentStamina;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        lastStaminaUseTime = Time.time;
    }

    public void Die()
    {
        Debug.Log("Player Died");
    }

    public bool CanUseStamina(float amount)
    {
        return currentStamina >= amount;
    }
}
