using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public List<GameObject> enemyPrefabs = new List<GameObject>(); // Danh sách các prefab enemy

    [Header("Spawn Settings")]
    public int spawnAmount = 1;         // Số lượng spawn mỗi lần
    public bool randomizeEnemy = true;  // Spawn random enemy hay không
    public static EnemySpawner Instance { get; private set; }

    private void Awake()
    {
        // Nếu chưa có Instance thì gán
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Nếu đã có, xóa bản thừa
        }
    }
    public void SpawnEnemies(Transform spawnPoint)
    {
        if (enemyPrefabs.Count == 0)
        {
            return;
        }

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject selectedEnemy = GetRandomEnemy();
            Instantiate(selectedEnemy, spawnPoint.position, Quaternion.identity);
        }
    }

    private GameObject GetRandomEnemy()
    {
        if (randomizeEnemy)
        {
            return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        }
        else
        {
            return enemyPrefabs[0]; // Nếu không random thì lấy enemy đầu tiên
        }
    }
}
