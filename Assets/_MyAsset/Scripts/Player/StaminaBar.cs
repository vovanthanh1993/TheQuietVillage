using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Image staminaBarIM;


    void Start()
    {
        staminaBarIM = GetComponent<Image>();

    }
    void Update()
    {
         staminaBarIM.fillAmount = PlayerHealth.Instance.GetStaminaFillAmount();
    }
}
