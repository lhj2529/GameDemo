using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikes : MonoBehaviour
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
            
            // 查找所有Ice组件（包括未激活的GameObject）
            IceSpikes[] iceComponents = Object.FindObjectsByType<IceSpikes>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
                );
            if (iceComponents.Length < 3)
            {
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 5, 0);
                Vector3 spikes_1 = new Vector3(target.transform.position.x + 2, target.transform.position.y + 5, 0);
                Vector3 spikes_2 = new Vector3(target.transform.position.x - 2, target.transform.position.y + 5, 0);
                Instantiate(this, spikes_1, Quaternion.identity);
                Instantiate(this, spikes_2, Quaternion.identity);
            }
        }
    }

    private void Attack()
    {


        // 计算移动方向
        Vector3 direction = new Vector3(0,-1,0);

        // 移动弹体
        transform.position += direction * speed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 检查碰撞对象的标签是否为 "boss"
        if (collision.CompareTag("boss"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(this.gameObject); // 销毁弹体对象
        }
        // 检查碰撞对象的标签是否为 "boss"
        if (collision.CompareTag("Ground"))
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            Destroy(this.gameObject); // 销毁弹体对象
        }
    }
}
