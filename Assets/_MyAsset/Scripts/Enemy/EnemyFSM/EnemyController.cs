using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyState currentState;

    public EnemyState CurrentState => currentState;

    [Header("References")]
    public Transform player;
    public UnityEngine.AI.NavMeshAgent agent;
    public EnemyAnimationHandle animationHandle;
    public EnemyHealth enemyHealth;

    [Header("Settings")]
    public float detectionRange = 10f;
    public float hearingRange = 30f;
    public float investigateTime = 3f;
    public float attackRange = 2f;
    public float attackRangeDamage = 3f;
    public float attackRate = 2f;
    public int attackDamage = 10;
    public float chaseRange = 10f;
    public float attackDelay = 1f;
    public float hitDelay = 0.5f;
    public float maxChaseTime = 3f;
    public List<Transform> waypoints = new List<Transform>();
    public bool isRecoveringFromHit = false;

    [HideInInspector] public int currentWaypointIndex;
    [HideInInspector] public Vector3 lastSoundPosition;

    void Start()
    {
        Transform parent = GameObject.Find("WayPoint").transform;
        foreach (Transform child in parent)
        {
            waypoints.Add(child);
        }

        player = PlayerManager.Instance.transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animationHandle = GetComponent<EnemyAnimationHandle>();
        enemyHealth = GetComponent<EnemyHealth>();

        ChangeState(new PatrolState(this));
    }

    void Update()
    {
        if (enemyHealth.health <= 0 && !(currentState is DieState))
        {
            ChangeState(new DieState(this));
        }

        currentState?.Update();
    }

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void GetDamage()
    {
        if (enemyHealth.health <= 0) return;
        ChangeState(new HitState(this));
        /*if (!(currentState is HitState))
        {
            ChangeState(new HitState(this));
        }*/
    }

    public void HearSound(Vector3 soundPos)
    {
        if (currentState is DieState) return;

        float distanceToSound = Vector3.Distance(transform.position, soundPos);
        if (distanceToSound > hearingRange) return;

        lastSoundPosition = soundPos;
        ChangeState(new InvestigateState(this));
    }
}
