using UnityEngine;

public class InvestigateState : EnemyState
{
    private float timer;

    public InvestigateState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        enemy.agent.SetDestination(enemy.lastSoundPosition);
        enemy.animationHandle.Run(enemy.agent);
        timer = 0f;
    }

    public override void Update()
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (distance <= enemy.detectionRange)
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
        {
            timer += Time.deltaTime;
            if (timer >= enemy.investigateTime)
            {
                enemy.ChangeState(new PatrolState(enemy));
            }
        }
    }
}
