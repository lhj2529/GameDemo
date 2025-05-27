using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private GameEventChannel events;
    [SerializeField] private BossData bossData;

    [SerializeField] private Transform target;

    //public Vector2 attackOffset = new Vector2(0.5f, 0);

    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    private Animator anim;
    private BoxCollider2D attackCollider;

    [Header("Attack Area")]
    [SerializeField] private Vector2 attackSize = new Vector2(0.76f, 0.08f);
    [SerializeField] private Vector2 attackOffset = new Vector2(0.2f, 0);



    private void Awake()
    {
        events.OnBossAttack.AddListener(AttackStart);  //设置监听事件
        events.OnBossAttack.AddListener(FaceTarget);

        anim = this.GetComponent<Animator>();
        attackCollider = attackPoint.GetComponent<BoxCollider2D>();
        ConfigureAttackCollider();
        attackCollider.enabled = false;
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        // 计算水平方向差
        float xDirection = target.position.x - transform.position.x;

        // 根据方向翻转Sprite
        if (xDirection > 0)
        {
            attackOffset = new Vector2(0.2f, 0);
        }
        else if (xDirection < 0)
        {
            attackOffset = new Vector2(-0.2f, 0);
        }
        ConfigureAttackCollider();
    }

    private void OnDestroy()
    {
        events.OnBossAttack.RemoveListener(AttackStart);
        events.OnBossAttack.RemoveListener(FaceTarget);
    }

    // 初始化攻击碰撞体参数
    private void ConfigureAttackCollider()
    {
        attackCollider.size = attackSize;
        attackCollider.offset = attackOffset;
    }
    void FaceTarget(Transform player)
    {
        //Vector3 direction = (player.position - transform.position).normalized;
        //Quaternion lookRotation = Quaternion.LookRotation(direction);
        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void AttackStart(Transform player)
    {
        // 触发攻击动画
        anim.SetTrigger("Attack");

        target = player;
    }

    private void PerformAttack()
    {


        if (attackCollider.enabled == true)
        { 
            // 获取碰撞体实际位置和旋转
            Vector2 position = attackPoint.TransformPoint(attackCollider.offset);
            // 检测敌人
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
                position,
                attackSize,
                0,
                enemyLayer
            );

            // 对每个敌人造成伤害
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.CompareTag("Player"))
                {
                    //enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                    Debug.Log("攻击到了玩家");
                }
            }
        }
    }

    // 动画事件调用方法（在攻击动画关键帧添加事件）
    public void EnableAttackCollider() 
    { 
        
        attackCollider.enabled = true;
        PerformAttack();
    }
    public void DisableAttackCollider() => attackCollider.enabled = false;



}
