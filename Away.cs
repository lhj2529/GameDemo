using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Away : MonoBehaviour
{

    [SerializeField] private GameEventChannel events;
    [SerializeField] private BossData bossData;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Awake()
    {
        events.OnBossAwayStart.AddListener(AwayStart);
        events.OnBossAwayStop.AddListener(AwayStop);
    }

    private void OnDestroy()
    {
        events.OnBossAwayStart.RemoveListener(AwayStart);
        events.OnBossAwayStop.RemoveListener(AwayStop);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }

    private void Update()
    {
        RotateTowardsTarget();
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

            // 计算远离方向（Boss位置 - 玩家位置的符号）
        float direction = Mathf.Sign(transform.position.x - playerTransform.position.x);
            // 设置刚体速度
        rb.velocity = new Vector2(direction * bossData.chaseSpeed, rb.velocity.y);

    }

    void AwayStart(Transform player)
    {
        playerTransform = player;
    }
    void AwayStop(Transform player)
    {
        playerTransform = null;
    }

    void RotateTowardsTarget()
    {
        if (playerTransform == null) return;

        // 计算水平方向差
        float xDirection = playerTransform.position.x - transform.position.x;

        // 根据方向翻转Sprite
        if (xDirection > 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = false; // 玩家在zuo侧

        }
        else if (xDirection < 0)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;  // 玩家在you侧
        }
    }
}
