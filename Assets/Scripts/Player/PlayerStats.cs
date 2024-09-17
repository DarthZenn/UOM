using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaRegenRate = 10f;

    public Slider healthSlider; // Reference to the Health UI Slider
    public Slider staminaSlider; // Reference to the Stamina UI Slider

    private bool isSprinting;

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // Initialize slider values
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    void Update()
    {
        // Update sliders in UI
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;

        // Regenerate stamina if not sprinting
        if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        if (Input.GetKeyDown(KeyCode.H)) // Press 'H' to take damage
        {
            TakeDamage(10f); // Decrease health by 10
        }
    }

    // Function to increase health
    public void RestoreHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // Function to use stamina
    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        if (currentStamina <= 0)
        {
            // Stop sprinting if stamina reaches 0
            isSprinting = false;
        }
    }

    // Function to deal damage to the player
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    // Player death handling
    void Die()
    {
        Debug.Log("Player has died!");
        // You can implement death behavior here (restart level, game over, etc.)
    }
}
