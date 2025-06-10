using System;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;

public static class PlayerEvents
{
    public static event Action OnTakeDamage;
    public static event Action OnDie;
    public static event Action OnHeal;
    public static event Action<float> OnStaminaChanged;
    public static event Action<float> OnHealthChanged;

    public static void TriggerTakeDamage(float value)
    {
        OnTakeDamage?.Invoke();
        OnHealthChanged?.Invoke(value);
    }
    public static void TriggerDie() => OnDie?.Invoke();
    public static void TriggerHeal() => OnHeal?.Invoke();
    public static void TriggerStaminaChange(float value) => OnStaminaChanged?.Invoke(value);
    public static void TriggerHealthChange(float value) => OnHealthChanged?.Invoke(value);
}
