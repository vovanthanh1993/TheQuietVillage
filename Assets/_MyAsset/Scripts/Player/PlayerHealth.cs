using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private float maxHealth = 100f;
    private bool isDead = false;

    void Start()
    {
        ResetHealth();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PlayerEvents.TriggerTakeDamage(currentHealth / maxHealth);

        if (currentHealth <= 0 && !isDead)
        {
            PlayerEvents.TriggerDie();
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        PlayerEvents.TriggerHeal();
        PlayerEvents.TriggerHealthChange(currentHealth / maxHealth);
    }

    public void SetHealth(float amount)
    {
        currentHealth = amount;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        PlayerEvents.TriggerHealthChange(currentHealth / maxHealth);
    }
}