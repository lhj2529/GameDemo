using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Boss Data")]
public class BossData : ScriptableObject
{

    public float chaseSpeed = 3.5f;  //追逐速度
    public float attackRange = 2f;  //攻击范围
    public int maxHealth = 100;  //最大生命值
    public int attackDamage = 20;  //攻击伤害

}
