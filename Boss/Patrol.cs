using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [Header("移动设置")]
    public float moveRadius = 5f;    // 巡逻半径
    public float speed = 3f;        // 移动速度
    public float minUpdateTime = 1f; // 最短目标更新间隔
    public float maxUpdateTime = 3f; // 最长目标更新间隔

    private Vector2 targetPosition;
    private bool isMoving = false;
    [SerializeField] private GameEventChannel events;

    private void Awake()
    {
        events.OnBossPatrolStart.AddListener(PatrolStart);
        events.OnBossPatrolStop.AddListener(PatrolStop);
    }

    private void OnDestroy()
    {
        events.OnBossPatrolStart.RemoveListener(PatrolStart);
        events.OnBossPatrolStop.RemoveListener(PatrolStop);
    }

    void Start()
    {
        SetNewRandomTarget();
        StartCoroutine(MovementRoutine());
    }

    System.Collections.IEnumerator MovementRoutine()
    {
        while (true)
        {
            // 等待随机时间后更新目标
            yield return new WaitForSeconds(Random.Range(minUpdateTime, maxUpdateTime));
            SetNewRandomTarget();
        }
    }

    void Update()
    {
        if (isMoving)
        {
            // 持续向目标位置移动
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            // 更新面向方向（可选）
            UpdateFacingDirection();
        }
    }

    void PatrolStart(Transform player)
    {
        isMoving = true;
    }
    void PatrolStop(Transform player)
    {
        isMoving = false;
    }

    void SetNewRandomTarget()
    {
        // 在移动半径范围内生成随机点
        float randomOffset = Random.Range(-moveRadius, moveRadius);
        targetPosition = new Vector2(randomOffset, transform.position.y);
    }

    void UpdateFacingDirection()
    {
        // 根据移动方向翻转Sprite
        if (targetPosition.x > transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = true; // 玩家在zuo侧
        }
        else if (targetPosition.x < transform.position.x)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;  // 玩家在you侧
        }
    }
}
