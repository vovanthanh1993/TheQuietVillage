using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public static PlayerItem Instance;
    [SerializeField] private GameObject flashlightItem;
    public GameObject flashlightAdvanced;
    [SerializeField] private GameObject pistolItem;
    [SerializeField] private GameObject pistolAnims;
    [SerializeField] private GameObject knifeItem;
    private HashSet<string> collectedKeys = new HashSet<string>();
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

    public void ResetPlayerItems()
    {
        if (flashlightItem != null)
        {
            flashlightAdvanced.GetComponent<FlashlightAdvanced>().Reset();
            flashlightItem.SetActive(true);
        }

        if (pistolItem != null)
        {
            pistolAnims.GetComponent<GunSystem>().Reset();
            pistolItem.gameObject.SetActive(false);
        }

        if (knifeItem != null)
        {
            knifeItem.gameObject.SetActive(false);
        }
    }

    public void UpdatePistolAmmo(int amount)
    {
        pistolAnims.GetComponent<GunSystem>().ammoCache+= amount;
    }

    public void AddKey(string keyID)
    {
        collectedKeys.Add(keyID);
    }

    public bool HasKey(string keyID)
    {
        return collectedKeys.Contains(keyID);
    }
}
