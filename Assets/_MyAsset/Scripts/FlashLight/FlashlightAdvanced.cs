using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Item;

public class FlashlightAdvanced : MonoBehaviour
{
    [Header("Flashlight Settings")]
    [SerializeField] private Light light;
    [SerializeField] private Animator animator;

    [Header("Battery and Lifetime Settings")]
    [SerializeField] private float lifetime = 100f; // Thời gian sử dụng flashlight (tính bằng giây)
    [SerializeField] private float batteries = 0f;  // Lượng pin hiện tại
    private bool on = false;
    private bool off = true;
    private bool  isReloading = false;

    void Start()
    {
        light = GetComponent<Light>();
        off = true;
        light.enabled = false;
    }

    void Update()
    {
        GUIManager.Instance.FlashLightUIUpdate(lifetime.ToString("0") + "%", batteries.ToString());
        if (InputManager.Instance.IsFlashLightClick() && off)
        {
            FlashLightOn();
        }
        else if (InputManager.Instance.IsFlashLightClick() && on)
        {
            FlashLightOff();
        }

        if (on)
        {
            lifetime -= 1 * Time.deltaTime;
        }

        if(lifetime <= 0)
        {
            light.enabled = false;
            on = false;
            off = true;
            lifetime = 0;
            GUIManager.Instance.ShowFlashLightOn(false);
        }

        if (lifetime >= 100)
        {
            lifetime = 100;
        }

        if (InputManager.Instance.IsReload() && batteries >= 1 && !isReloading)
        {
            Reload();
        }

        if (InputManager.Instance.IsReload() && batteries == 0)
        {
            return;
        }

        if(batteries <= 0)
        {
            batteries = 0;
        }
    }

    public void Reset()
    {
        lifetime = 100;
        batteries = 0;
        on = false;
        off = true;
        light.enabled = false;
    }

    private void Reload()
    {
        StartCoroutine(ReloadTimer());
    }

    IEnumerator ReloadTimer()
    {
        isReloading = true;
        FlashLightOff();
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(6);
        FlashLightOn();
        batteries -= 1;
        lifetime += 50;
        InventoryManager.Instance.RemoveItem(ItemType.Battery);
        isReloading = false;
    }

    private void FlashLightOn()
    {
        AudioManager.Instance.FlashLightOnSound();
        light.enabled = true;
        on = true;
        off = false;
        GUIManager.Instance.ShowFlashLightOn(true);
    }

    private void FlashLightOff()
    {
        AudioManager.Instance.FlashLightOffSound();
        light.enabled = false;
        on = false;
        off = true;
        GUIManager.Instance.ShowFlashLightOn(false);
    }

    private void OnEnable()
    {
        GUIManager.Instance.ShowFlashLightGUI(true);
    }

    private void OnDisable()
    {
        GUIManager.Instance.ShowFlashLightGUI(false);
        if (isReloading)
        {
            StopCoroutine("ReloadTimer");
            isReloading = false;
        }
    }

    public float GetLifetime()
    {
        return lifetime;
    }

    public void AddBatteries(int amount)
    {
        batteries += amount;
    }

}
