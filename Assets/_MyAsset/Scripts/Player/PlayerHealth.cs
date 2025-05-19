using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    private float maxHealth = 100f;
    private float stamina = 100f;
    private float maxStamina = 100f;
    [SerializeField] private float staminaDrainRate = 20f; // Stamina mất mỗi giây khi chạy
    [SerializeField] private float staminaRegenRate = 10f; // Stamina hồi mỗi giây khi không chạy
    [SerializeField] private float sprintMinStamina = 40f; // Bao nhiêu % stamina mới cho chạy lại
    [SerializeField] AudioClip cantSprintSound;
    private bool canSprint = true;


    public static PlayerHealth Instance;

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của PlayerManager trong game.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ player qua các scene.
        }
        else
        {
            Destroy(gameObject); // Nếu đã có instance khác, xóa đối tượng hiện tại.
        }
    }
    void Update()
    {
        if(health <= 0)
        {
            GetComponent<PlayerController>().enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GUIManager.Instance.ShowHUD(false);
            gameObject.SetActive(false);
            PauseMenuManager.Instance.ShowGameOverPanel(true);
        }

        if (health > 100)
        {
            health = 100;
        }
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage * SettingsMenuManager.Instance.DamageMultiplierSetting;
        AudioManager.Instance.HurtSound();
        GUIManager.Instance.ShowScreenTakeDmg();
    }

    public void DrainStamina()
    {
        stamina -= staminaDrainRate * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        if (stamina <= 0f)
        {
            canSprint = false;
            AudioManager.Instance.PlayEffect(cantSprintSound);
        }
            
    }

    public void RegenStamina()
    {
        stamina += staminaRegenRate * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
        if (!canSprint && stamina >= sprintMinStamina)
        {
            canSprint = true;
        }
    }

    public float GetStamina()
    {
        return stamina;
    }

    public float GetHealth()
    {
        return health;
    }

    public void AddHealth(float amount)
    {
        health+= amount;
        AudioManager.Instance.HealthPickupSound();
        GUIManager.Instance.ShowScreenHealing();
    }

    public void SetHealth(float amount)
    {
        health = amount;
    }

    public float GetStaminaFillAmount()
    {
        return stamina / maxStamina;
    }

    public float GetHealthFillAmount()
    {
        return health / maxHealth;
    }

    public bool CanSprint() => canSprint;

    public void Reset()
    {
        health = maxHealth;
        stamina = maxStamina;
    }
}
