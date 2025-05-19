using UnityEngine;
using System.Collections;

public class AttackState : EnemyState
{
    public AttackState(EnemyController enemy) : base(enemy) { }
    private Coroutine attackCoroutine;
    public override void Enter()
    {
        enemy.agent.isStopped = true;
        attackCoroutine = enemy.StartCoroutine(PerformAttack());
    }

    public override void Exit()
    {
        enemy.agent.isStopped = false;
        if (attackCoroutine != null)
        {
            enemy.StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    private IEnumerator PerformAttack()
    {
        enemy.animationHandle.Attack(enemy.agent);
        yield return new WaitForSeconds(enemy.attackDelay);
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (distance <= enemy.attackRangeDamage)
        {
            PlayerHealth playerHealth = enemy.player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemy.attackDamage);
            }
        }

        yield return new WaitForSeconds(enemy.attackRate);
        enemy.ChangeState(new ChaseState(enemy));
    }
}
