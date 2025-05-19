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
    void Update()
    {
        healthBarIM.fillAmount = PlayerHealth.Instance.GetHealthFillAmount();
    }
}
