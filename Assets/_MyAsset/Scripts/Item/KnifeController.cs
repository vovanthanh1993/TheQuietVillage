using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Parameters")]
    private static readonly int Draw = Animator.StringToHash("Draw");
    private static readonly int Hide = Animator.StringToHash("Hide");
    private static readonly int AttackType = Animator.StringToHash("AttackType");
    private static readonly int Attack = Animator.StringToHash("Attack");

    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;

    [Header("Attack Settings")]
    [SerializeField] private float delayAttack = 1f;     // Độ trễ giữa các đòn đánh
    [SerializeField] private int damage = 25;
    [SerializeField] private float range = 2f;
    private bool canAttack = true;

    [Header("References")]
    [SerializeField] private GameObject fpsCam;

    [SerializeField] private GameObject impactEffectFlesh;
    [SerializeField] private GameObject impactEffectTerrain;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {

        if (!canAttack) return;

        if (InputManager.Instance.IsShooting()) // chuột trái chém 1
        {
            animator.SetTrigger(Attack);
            animator.SetInteger(AttackType, Random.Range(0, 3));
            canAttack = false;
            AudioManager.Instance.PlayEffect(attackSound);
            Invoke(nameof(ResetAttack), delayAttack); // delay dựa vào animation length
            
        }
    }

    /*void DrawKnife()
    {
        animator.SetTrigger(Draw);
        isKnifeOut = true;
    }

    void HideKnife()
    {
        animator.SetTrigger(Hide);
        isKnifeOut = false;
    }*/

    void ResetAttack()
    {
        canAttack = true;
    }

    public void DealDamageToEnemy()
    {
        RaycastHit hit;
        int layerToIgnore = LayerMask.GetMask("IgnoreRaycast");
        int layerMask = ~layerToIgnore;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * range, Color.red, 1f);

            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            GameObject impactOB;
            if (target != null)
            {
                target.TakeDamage(damage);
                impactOB = Instantiate(impactEffectFlesh, hit.point, Quaternion.LookRotation(hit.normal));
            }
            else
            {
                impactOB = Instantiate(impactEffectTerrain, hit.point, Quaternion.LookRotation(hit.normal));
            }
            Destroy(impactOB, 2f);
        }
    }
    private void OnEnable()
    {
        GUIManager.Instance.ShowKnifeUI(true);
    }

    private void OnDisable()
    {
        GUIManager.Instance.ShowKnifeUI(false);
    }
}
