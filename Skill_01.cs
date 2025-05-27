using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_01 : MonoBehaviour
{
    [SerializeField] private float coolDownDuration = 5f;   //技能冷却时间


    private bool isCoolingDown=false;   //技能冷却标志
    private float cooldownTimer = 0f;    //冷却计时器
    private float comboStage = 1f;
    private Transform enemyPos = null;

    private GameObject Skill_03_Target = null;
    [SerializeField] private GameObject Skill_03_Sword_Prefab ;
    [SerializeField] private Transform playerPos ;
    private CharacterController cc;
    [SerializeField] private Animator animator1;

    [SerializeField] private GameEventChannel events;

    //三阶段技能协程所用的属性
    public float dashSpeed = 10f;       // 冲刺速度（米/秒）
    public float stoppingDistance = 0.1f; // 停止距离阈值
    public float gravity = -9.81f;      // 重力加速度
    private bool isDashing = false;     // 是否正在冲刺

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        events.OnPassComboStage.AddListener(ReceiveStage);
        events.OnFindNearestEnemy.AddListener(ReceiveEnemyPos);
        events.OnFindCenterEnemy.AddListener(ReceiveSwordPos);
    }

    // Update is called once per frame
    void Update()
    {
        //更新冷却计时器
        if (isCoolingDown)
        {
            cooldownTimer -= Time.deltaTime;
            if(cooldownTimer <= 0f)
                isCoolingDown=false;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            TryCastSkill();
        }

    }

    void TryCastSkill()
    {
        if (!isCoolingDown&& !isDashing)
        {
            CastSkill();
            StartCooldown();
        }
    }

    void StartCooldown()
    {
        isCoolingDown = true;
        cooldownTimer = coolDownDuration;
    }

    void CastSkill()
    {
        switch (comboStage)
        {
            case 1f:
                
                CastSkill_01();
                break;

            case 2f:
                CastSkill_02();
                break;

            case 3f:
                CastSkill_03();
                break;
        }
    }

    void CastSkill_01()
    {
        events.OnApplicationFind_Nearest?.Invoke();
        events.OnAttackColliderCheck?.Invoke(5);

        Vector3 direction = (playerPos.position - enemyPos.position).normalized;
        Vector3 teleportPosition = enemyPos.position + direction * 2f;
        teleportPosition.y = playerPos.position.y;

        //与角色控制器产生冲突，先禁用其，再执行瞬移，再启用
        cc.enabled = false;
        playerPos.position = teleportPosition;
        cc.enabled = true;

        animator1.Play("Skill_01_01");
    }

    void CastSkill_02()
    {
        events.OnAttackColliderCheck?.Invoke(6);
        animator1.CrossFade("Skill_01_02",0.03f);
    }

    void CastSkill_03()
    {

        for (int i = 0; i < 6; i++)
        {
            // 计算当前角度（度数）并转换为弧度
            float angleDegrees = 90f + 60f * i;
            float radians = Mathf.Deg2Rad * angleDegrees;

            // 计算相对玩家的坐标
            float x = Mathf.Cos(radians) * 10f;
            float z = Mathf.Sin(radians) * 10f;

            // 确定生成位置（保持与玩家相同的Y轴高度）
            Vector3 spawnPosition = new Vector3(
                playerPos.position.x + x,
                playerPos.position.y,
                playerPos.position.z + z
            );

            // 生成物体并保持默认旋转
            Instantiate(Skill_03_Sword_Prefab, spawnPosition, Skill_03_Sword_Prefab.transform.rotation);
        }

        StartCoroutine(DashToPositionCoroutine());

    }

    // 协程处理冲刺逻辑
    private IEnumerator DashToPositionCoroutine()
    {
        isDashing = true;
        Vector3 targetPosition;
        float verticalVelocity = 0f;

        GameObject[] Swords = GameObject.FindGameObjectsWithTag("Sword");
        for (int i = 0; i < Swords.Length; i++)
        {
            Destroy(Swords[i], 15f);
        }

        while (true)
        {
            if (Swords.Length <= 0)
            {
                events.OnResetCombo?.Invoke();
                Debug.Log("chongzhi请求");
                break;
            }
            else
            {
                Swords = GameObject.FindGameObjectsWithTag("Sword");

                if (Input.GetKeyUp(KeyCode.E))
                {
                    events.OnApplicationFind_Center?.Invoke();

                    targetPosition = Skill_03_Target.transform.position;
                    events.OnAttackColliderCheck?.Invoke(7);
                    animator1.Play("Skill_01_03");

                    while (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
                    {
                        // 计算水平移动方向
                        Vector3 horizontalDirection = (targetPosition - transform.position).normalized;
                        horizontalDirection.y = 0; // 忽略垂直分量

                        // 计算水平位移
                        Vector3 horizontalMove = horizontalDirection * dashSpeed * Time.deltaTime;

                        // 处理重力
                        if (!cc.isGrounded)
                        {
                            verticalVelocity += gravity * Time.deltaTime;
                        }
                        else
                        {
                            verticalVelocity = -0.1f; // 轻微向下速度防止漂浮
                        }

                        // 合并垂直位移
                        Vector3 verticalMove = new Vector3(0, verticalVelocity * Time.deltaTime, 0);

                        // 移动角色
                        cc.Move(horizontalMove + verticalMove);

                        yield return null; // 等待下一帧
                    }


                    // 冲刺结束后，精准对齐目标位置
                    transform.position = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
                }
                yield return null; // 等待下一帧
            }
        }



        isDashing = false;
    }

    void ReceiveStage(float comboStage_)
    {
        comboStage=comboStage_;
    }

    void ReceiveEnemyPos(Transform enemypos)
    {
        enemyPos = enemypos;
    }

    void ReceiveSwordPos(GameObject Sword)
    {
        Skill_03_Target=Sword;
    }
}
