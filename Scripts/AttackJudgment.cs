using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJudgment : MonoBehaviour
{
    private bool canAttack = true;

    [SerializeField] private GameEventChannel events;

    //攻击碰撞箱
    [SerializeField] private GameObject attackCollider;
    private float attackType;
    private Vector3[] attack_Size =new Vector3[]    // 碰撞箱尺寸数组
    {
        //普通攻击四段的碰撞箱尺寸
        new Vector3(1.3845f, 0.9134f, 0.9223f), 
        new Vector3(1.1631f, 0.9134f, 0.9223f),
        new Vector3(1.6687f, 0.9134f, 1.4948f),
        new Vector3(2.1548f, 0.9134f, 1.7252f),

        //阶段一技能碰撞箱尺寸
        new Vector3(1.9730f, 2.0031f, 1.9608f),

        //阶段二技能的两段碰撞箱尺寸
        new Vector3(2.2955f, 1.2575f, 2.6444f),

        //阶段三技能的碰撞箱
        new Vector3(1.9730f, 1.3176f, 1.9608f),

    }; 

    // Start is called before the first frame update
    void Start()
    {
        attackCollider.GetComponent<BoxCollider>().enabled = false;   //初始碰撞关闭
        events.OnAttackColliderCheck.AddListener(ReceiveCollider);
    }

    private void ReceiveCollider(float attackType_)
    {
        attackType=attackType_-1;
    }

    public void StartAttack()
    {
        if (!canAttack)
            return;
        attackCollider.GetComponent<BoxCollider>().enabled = true;
        attackCollider.GetComponent<BoxCollider>().size = attack_Size[(int)attackType];
        canAttack = false;
    }

    public void EndAttack()
    {
        attackCollider.GetComponent<BoxCollider>().enabled = false;
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("攻击到了敌人");
            events.OnAttackHits?.Invoke();
        }
    }
}
