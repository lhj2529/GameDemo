using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    [SerializeField] private GameEventChannel events;

    [SerializeField] private BossData bossData;

    [SerializeField] Rigidbody2D rb;             // Boss的刚体组件
    private Transform target;
    [SerializeField] SpriteRenderer spriteRenderer; // 精灵渲染器
    [SerializeField] private Transform attackPoint;


    private void Awake()
    {
        events.OnBossChaseStart.AddListener(StartChasing);
        events.OnBossChaseStop.AddListener(StopChasing);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        events.OnBossChaseStart.RemoveListener(StartChasing);
        events.OnBossChaseStop.RemoveListener(StopChasing);
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            MoveTowardsTarget();
            RotateTowardsTarget();
        }
    }

    void StartChasing(Transform player)
    {
        target = player;
        // 可以在此处触发追击特效或声音
    }

    void StopChasing(Transform player)
    {
        target = null;
        rb.velocity = Vector2.zero;
        // 可以在此处触发停止追击动画
    }

    void MoveTowardsTarget()
    {
        float direction = Mathf.Sign(target.position.x - transform.position.x);
        rb.velocity = new Vector2(direction * bossData.chaseSpeed, rb.velocity.y);
    }

    void RotateTowardsTarget()
    {
        if (target == null) return;

        // 计算水平方向差
        float xDirection = target.position.x - transform.position.x;

        // 根据方向翻转Sprite
        if (xDirection > 0)
        {
            spriteRenderer.flipX = true; // 玩家在zuo侧

            attackPoint.localPosition=new Vector3((float)0.2, 0, 0) ;
        }
        else if (xDirection < 0)
        {
            spriteRenderer.flipX = false;  // 玩家在you侧

            attackPoint.localPosition = new Vector3((float)-0.2, 0, 0);
        }
    }
}
