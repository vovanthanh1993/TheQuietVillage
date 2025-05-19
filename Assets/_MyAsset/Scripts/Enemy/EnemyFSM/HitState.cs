using UnityEngine;
using System.Collections;

public class HitState : EnemyState
{
    public HitState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.isRecoveringFromHit = false;
        enemy.agent.isStopped = true;
        enemy.animationHandle.Hurt();
        enemy.StartCoroutine(RecoverFromHit());
    }

    private IEnumerator RecoverFromHit()
    {
        yield return new WaitForSeconds(enemy.hitDelay);
        enemy.agent.isStopped = false;
        enemy.isRecoveringFromHit = true;
        enemy.ChangeState(new ChaseState(enemy));
    }


}
