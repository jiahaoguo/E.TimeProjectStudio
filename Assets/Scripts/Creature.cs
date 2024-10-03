using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creature : MonoBehaviour
{
    public int MaxHealth;
    private int health;

    [SerializeField] private Slider healthSlider;

    // Property for health, with the slider value being updated when health changes
    public int Health
    {
        get => health;
        set
        {
            health = Mathf.Clamp(value, 0, MaxHealth); // Ensure health doesn't exceed bounds
            UpdateHealthSlider();

            // Check if health is 0, if so destroy the object
            if (health <= 0)
            {
                Destroy(gameObject);
                Debug.Log($"{gameObject.name} has been destroyed.");
            }
        }
    }

    void Start()
    {
        // Initialize the health and the slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = MaxHealth;
            Health = MaxHealth; // Set initial health to max
            Debug.Log("Creature initialized.");
        }
    }

    // Method to update the slider based on health
    private void UpdateHealthSlider()
    {
        healthSlider.value = (float)health;
    }
}
