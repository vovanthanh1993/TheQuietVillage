using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [Header("Health Settings")]
    public int addHealth = 25; // Lượng sức khỏe thêm vào
    private float currentHealth; // Sức khỏe hiện tại của player

    [Header("Health Bar Settings")]
    public GameObject healthBar; // GameObject thanh sức khỏe, cần gán trong Unity Editor

    [Header("Interaction Settings")]
    public bool inReach; // Biến xác định liệu người chơi có ở gần vật phẩm hồi phục hay không

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            GUIManager.Instance.ShowPickUpText(true);

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            GUIManager.Instance.ShowPickUpText(false);
            GUIManager.Instance.ShowCantPickUpText(false);
        }
    }

    void Start()
    {
        inReach = false;
    }

    void Update()
    {
        currentHealth = PlayerHealth.Instance.GetHealth();
        if (inReach && InputManager.Instance.IsInteract() && currentHealth < 100)
        {
            inReach = false;
            AudioManager.Instance.HealthPickupSound();
            PlayerHealth.Instance.AddHealth(addHealth);
            GUIManager.Instance.ShowScreenHealing();
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GUIManager.Instance.ShowPickUpText(false);
        }

        else if (inReach && InputManager.Instance.IsInteract() && currentHealth == 100)
        {
            GUIManager.Instance.ShowPickUpText(false);
            GUIManager.Instance.ShowCantPickUpText(true);
        }

    }

    
}
