using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyController enemy) : base(enemy) { }
    private float chaseStarTime = 0f;
    public override void Enter()
    {
        enemy.animationHandle.Run(enemy.agent);
        chaseStarTime = Time.time;
    }

    public override void Update()
    {
        enemy.agent.SetDestination(enemy.player.position);
        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (distance <= enemy.attackRange && enemy.agent.remainingDistance <= enemy.attackRange)
        {
            enemy.ChangeState(new AttackState(enemy));
            return;
        }

        // Nếu chưa đủ thời gian ép buộc thì tiếp tục đuổi không kiểm tra khoảng cách
        if (Time.time - chaseStarTime < enemy.maxChaseTime)
            return;

        // Sau maxChaseTime mới bắt đầu kiểm tra khoảng cách
        if (distance > enemy.chaseRange)
        {
            enemy.ChangeState(new PatrolState(enemy));
            return;
        }

        
    }
}
