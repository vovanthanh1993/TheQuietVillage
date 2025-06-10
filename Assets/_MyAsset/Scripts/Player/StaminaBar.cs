using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StaminaBar : MonoBehaviour
{
    private Image staminaBarIM;

    private void Start()
    {
        staminaBarIM = GetComponent<Image>();
        PlayerEvents.OnStaminaChanged += UpdateBar;
    }

    private void UpdateBar(float percent)
    {
        staminaBarIM.fillAmount = percent;
    }

    private void OnDisable()
    {
        PlayerEvents.OnStaminaChanged -= UpdateBar;
    }
}
