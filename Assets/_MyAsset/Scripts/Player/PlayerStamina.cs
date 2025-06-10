using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float currentStamina = 100f;
    public float staminaDrainRate = 10f;
    public float staminaRegenRate = 20f;
    public float sprintMinStamina = 40f;

    [Header("Audio")]
    public AudioClip cantSprintSound;

    private bool canSprint = true;
    private bool hasPlayedCantSprintSound = false;

    public event Action<float> OnStaminaChanged;

    private PlayerController playerController;

    public bool CanSprint { get ; set ; }

    private void Start()
    {
        currentStamina = maxStamina;
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.IsSprinting && CanSprint)
            DrainStamina();
        else
            RegenStamina();
    }

    public void DrainStamina()
    {
        currentStamina -= staminaDrainRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        PlayerEvents.TriggerStaminaChange(currentStamina / maxStamina);

        if (currentStamina <= 0f && CanSprint)
        {
            CanSprint = false;

            if (!hasPlayedCantSprintSound)
            {
                AudioManager.Instance.PlayEffect(cantSprintSound);
                hasPlayedCantSprintSound = true;
            }
        }
    }

    public void RegenStamina()
    {
        currentStamina += staminaRegenRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        PlayerEvents.TriggerStaminaChange(currentStamina / maxStamina);

        if (!CanSprint && currentStamina >= sprintMinStamina)
        {
            CanSprint = true;
            hasPlayedCantSprintSound = false;
        }
    }
}
