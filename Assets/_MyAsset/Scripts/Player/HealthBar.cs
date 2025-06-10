using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private Image healthBarIM;

    void Start()
    {
        healthBarIM = GetComponent<Image>();
    }

    // Make this method public and match the event signature
    public void UpdateHealthBar(float value)
    {
        healthBarIM.fillAmount = value;
    }

    void OnEnable()
    {
        // Subscribe to the event
        PlayerEvents.OnHealthChanged += UpdateHealthBar;
    }
    void OnDisable()
    {
        // Unsubscribe from the event
        PlayerEvents.OnHealthChanged -= UpdateHealthBar;
    }
}
