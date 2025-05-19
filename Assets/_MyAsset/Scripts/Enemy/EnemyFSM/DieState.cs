using UnityEngine;

public class DieState : EnemyState
{
    public DieState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.isStopped = true;
        enemy.animationHandle.Die();
        //enemy.agent.enabled = false;

        if (enemy.TryGetComponent(out Collider col))
        {
            Object.Destroy(col);
        }
    }
}
