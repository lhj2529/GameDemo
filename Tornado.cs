using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{

    [SerializeField] private float baseDamage; //基础伤害
    [SerializeField] private float proDamage; //强化伤害
    [SerializeField] private float speed; //弹道速度
    [SerializeField] private GameObject effect;
    private GameObject target;
    private enum Attribute
    {
        fire,
        ice,
        wind
    }
    [SerializeField] private Attribute attribute; //法术属性
    private float timeInterval ;
    private bool isAttack=false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChooseTarget();
        Attack();
    }

    private void OnDestroy()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
    }

    private void ChooseTarget()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("boss");
        }
    }

    private void Attack()
    {
        // 计算移动方向
        Vector3 direction = (target.transform.position - transform.position).normalized;

        // 移动弹体
        transform.position += direction * speed * Time.deltaTime;
        

        if(isAttack && Time.time - timeInterval >= 0.5)
        {
            Debug.Log("攻击到敌人");
            Instantiate(effect, transform.position, Quaternion.identity);
            timeInterval = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞对象的标签是否为 "boss"
        if (collision.CompareTag("boss"))
        {
            timeInterval = Time.time;
            isAttack =true;
            Destroy(this.gameObject, 2); // 销毁弹体对象
        }
    }
}
