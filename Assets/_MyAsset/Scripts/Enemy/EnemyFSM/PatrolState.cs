using UnityEngine;

public class PatrolState : EnemyState
{
    public PatrolState(EnemyController enemy) : base(enemy) { }

    public override void Enter()
    {
        MoveToNextWaypoint();
        enemy.animationHandle.Walk(enemy.agent);
    }

    public override void Update()
    {
        if (enemy.player == null) return;

        float distance = Vector3.Distance(enemy.transform.position, enemy.player.position);
        if (distance <= enemy.detectionRange)
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
        {
            MoveToNextWaypoint();
        }
    }

    private void MoveToNextWaypoint()
    {
        if (enemy.waypoints.Count == 0) return;

        enemy.currentWaypointIndex = Random.Range(0, enemy.waypoints.Count);
        enemy.agent.SetDestination(enemy.waypoints[enemy.currentWaypointIndex].position);
    }
}
