using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private EnemyController enemyController;

    [Header("Health Settings")]
    [SerializeField] public float health = 100f;

    public void TakeDamage(float amount)
    {

        if (health <= 0f || enemyController == null) return;

        health -= amount;

        if (health > 0f)
        {
            //enemyController.GetDamage();
            enemyController.ChangeState(new HitState(enemyController));
        }
        else
        {
            enemyController.ChangeState(new DieState(enemyController));
        }
    }

}
