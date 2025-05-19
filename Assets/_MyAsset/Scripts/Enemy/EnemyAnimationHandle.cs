using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationHandle : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 0.5f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;
    [SerializeField] private AudioClip attackSound;

    public Animator animator;

    private bool isRunning;
    private bool isWalking;
    private bool isIdle;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        isIdle = false;
        isRunning = false;
        isWalking = true;
    }

    void Update()
    {
        HanleAnimation();
    }

    public void Run(NavMeshAgent agent)
    {
        if (isDead) return;
        agent.speed = runSpeed;
        isRunning = true;
        isWalking = false;
        isIdle = false;
    }

    public void Walk(NavMeshAgent agent)
    {
        if (isDead) return;
        agent.speed = walkSpeed;
        isRunning = false;
        isWalking = true;
        isIdle = false;
    }

    public void Idle(NavMeshAgent agent)
    {
        if (isDead) return;
        agent.speed = 0f;
        isRunning = false;
        isWalking = false;
        isIdle = true;
    }

    public void Attack(NavMeshAgent agent)
    {
        if (isDead) return;
        agent.speed = 0f;
        transform.LookAt(PlayerManager.Instance.player.transform);
        animator.SetTrigger("Attack");
        isRunning = false;
        isWalking = false;
        isIdle = false;
        AudioManager.Instance.PlayEffect(attackSound, transform);
    }

    public void Die()
    {
        isDead = true;
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Attack");

        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
        animator.SetBool("Idle", false);

        animator.SetTrigger("Die");
        AudioManager.Instance.PlayEffect(dieSound, transform);

        isRunning = false;
        isWalking = false;
        isIdle = false;
    }

    public void Hurt()
    {
        if (isDead) return;
        AudioManager.Instance.PlayEffect(hurtSound, transform);
        animator.SetTrigger("Hit");
        isRunning = false;
        isWalking = false;
        isIdle = false;
    }

    private void HanleAnimation()
    {
        if (isDead) return;
        animator.SetBool("Run", isRunning);
        animator.SetBool("Walk", isWalking);
        animator.SetBool("Idle", isIdle);
    }
}