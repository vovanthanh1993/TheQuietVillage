using UnityEngine;
using UnityEngine.UI;

public class BatteryBar : MonoBehaviour
{
    [Header("Flashlight Settings")]
    private Image batteryBarIM;

    [Header("Battery Settings")]
    [SerializeField] private float currentBattery;
    [SerializeField] private float maxBattery = 100f;

    private bool isFlashlightOn = false;

    void Start()
    {
        batteryBarIM = GetComponent<Image>();
    }



    void Update()
    {
        currentBattery = PlayerItem.Instance.flashlightAdvanced.GetComponent<FlashlightAdvanced>().GetLifetime();
        batteryBarIM.fillAmount = currentBattery / maxBattery;
    }
}
