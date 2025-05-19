using Unity.VisualScripting;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public Transform position;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Kiểm tra nếu đối tượng va chạm có tag "Player"
        {
            EnemySpawner.Instance.SpawnEnemies(position);
            Destroy(gameObject);
        }
    }
}
