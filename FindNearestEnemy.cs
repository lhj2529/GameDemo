using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNearestEnemy : MonoBehaviour
{
    [SerializeField] private GameEventChannel events;

    private Camera mainCamera;
    public GameObject currentTarget=null; // 当前锁定的目标
    public float maxLockDistance = 100f; // 最大锁定距离（3D空间中的距离）
    public LayerMask obstructionLayers; // 遮挡检测的层级

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        events.OnApplicationFind_Nearest.AddListener(FindEnemy_Nearest);
        events.OnApplicationFind_Center.AddListener(FindClosestTarget);
    }

    //就近索敌方式
    void FindEnemy_Nearest()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length > 0)
        {
            Transform nearest = null;
            float minDistance = Mathf.Infinity;   //设置初始索敌范围（此处为无限制）

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(
                    this.transform.position,
                    enemy.transform.position
                    );

                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy.transform;
                }
            }
            events.OnFindNearestEnemy?.Invoke(nearest);
        }
        else
        {
            Debug.Log("未找到敌人");
        }
    }

    //根据准星索敌方式
    void FindClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Sword");
        float minScreenDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                // 计算敌人到摄像机的3D距离
                float distanceToCamera = Vector3.Distance(mainCamera.transform.position, enemy.transform.position);
                if (distanceToCamera > maxLockDistance) continue;

                // 转换到屏幕坐标
                Vector3 screenPos = mainCamera.WorldToScreenPoint(enemy.transform.position);

                // 检查是否在摄像机前方
                if (screenPos.z <= 0) continue;

                //// 检查是否可见（无遮挡）
                //if (IsTargetObstructed(enemy.transform)) continue;

                // 计算屏幕中心距离
                float screenDistance = Vector2.Distance(
                    new Vector2(screenPos.x, screenPos.y),
                    new Vector2(screenCenter.x, screenCenter.y)
                );

                // 更新最近目标
                if (screenDistance < minScreenDistance)
                {
                    minScreenDistance = screenDistance;
                    closestEnemy = enemy;
                }
            }

            currentTarget = closestEnemy;
            events.OnFindCenterEnemy?.Invoke(currentTarget);
            // 触发锁定事件，例如更新UI
        }
        else
            Debug.Log("未找到目标");
    }
}
