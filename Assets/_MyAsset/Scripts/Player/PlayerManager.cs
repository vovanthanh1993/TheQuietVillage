using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Transform player;
    
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

    public void InitPlayer()
    {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPos");
        transform.position = spawnPoint.transform.position;
    }
    public void ResetPlayer()
    {
        InitPlayer();
        gameObject.SetActive(true);
        GetComponent<PlayerController>().enabled = true;
        PlayerHealth.Instance.Reset();
        PlayerItem.Instance.ResetPlayerItems();
    }
}
