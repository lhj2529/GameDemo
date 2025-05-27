using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public enum BossState { Idle, Chase, Attack, Dead , Patrol, Hit, Away }
    [SerializeField] BossState currentState = BossState.Idle;

    [SerializeField] private BossData bossData;


    [Header("References")]
    public Transform player;  //玩家游戏对象
    public Animator animator;  //绑定的动画器组件
    public Slider healthBar;  //血条UI

    [SerializeField] private GameEventChannel events;
    //private NavMeshAgent agent;  //导航网格代理
    private int currentHealth;  //当前生命值
    private float attackCooldown = 10f;  //攻击冷却时间
    private float lastAttackTime;  //最近一次攻击时间

    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        currentHealth = bossData.maxHealth;
        healthBar.maxValue = bossData.maxHealth;
        healthBar.value = bossData.maxHealth;
    }

    void Update()
    {
        if (currentState == BossState.Dead) return;

        UpdateState();
        UpdateAnimations();
    }

    void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case BossState.Idle:
                    ChangeState(BossState.Chase);
                break;

            case BossState.Chase:
                //agent.SetDestination(player.position);
                //agent.speed = chaseSpeed;

                events.OnBossChaseStart?.Invoke(player);


                if (distanceToPlayer <= bossData.attackRange && Time.time > lastAttackTime + attackCooldown)  //条件：进入攻击范围 && 攻击就绪
                {
                    events.OnBossChaseStop?.Invoke(player);
                    ChangeState(BossState.Attack); 
                }
                else if (Time.time < lastAttackTime + attackCooldown && distanceToPlayer <= bossData.attackRange) //条件：进入攻击范围 && 攻击未就绪
                {
                    events.OnBossChaseStop?.Invoke(player);
                    ChangeState(BossState.Away); 
                }
                break;

            case BossState.Attack:
                //FaceTarget();
                //AttackPlayer();
                
                events.OnBossAttack?.Invoke(player);
                lastAttackTime = Time.time;

                ChangeState(BossState.Away);
                break;

            case BossState.Away:
                if (Time.time - lastAttackTime > 1)
                {
                    events.OnBossAwayStart?.Invoke(player);

                    if (distanceToPlayer >= 10f)
                    {
                        events.OnBossAwayStop?.Invoke(player);
                        ChangeState(BossState.Patrol);
                    }
                }
                break;

            case BossState.Patrol:
                events.OnBossPatrolStart?.Invoke(player);

                if (Time.time > lastAttackTime + attackCooldown)  //条件：攻击就绪
                {
                    events.OnBossPatrolStop?.Invoke(player);
                    ChangeState(BossState.Chase);
                }
                break;
        }
    }

    void ChangeState(BossState newState)
    {
        currentState = newState;
        //agent.isStopped = (currentState == BossState.Attack);
    }

    void UpdateAnimations()
    {
        animator.SetBool("isFindPlayer", currentState == BossState.Idle);
        animator.SetBool("isChase", currentState == BossState.Chase);
        
        animator.SetBool("isAway", currentState == BossState.Away);
        animator.SetBool("isPatrol", currentState == BossState.Patrol);
        //animator.SetTrigger("HitTrigger"); // 受击时调用
        //animator.SetTrigger("DieTrigger"); // 死亡时调用
    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(int damage)
    {
        if (currentState == BossState.Dead) return;

        currentHealth -= damage;
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        ChangeState(BossState.Dead);
        animator.SetTrigger("Die");
        //agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        enabled = false;
    }

    // 在攻击动画关键帧调用
    void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= bossData.attackRange)
        {
            //player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }
}
