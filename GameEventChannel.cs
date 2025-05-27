using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Events/Game Event Channel")]
public class GameEventChannel : ScriptableObject
{

    //////////////////////Player行为事件////////////////////////////

    //指令输入事件（携带原始输入字符串）
    public UnityEvent<string> OnSpellInput = new UnityEvent<string>();

    //有效指令存储事件（携带验证后的指令字符串）
    public UnityEvent<string> OnStoreSpell = new UnityEvent<string>();

    //法术释放事件（携带某一技能槽对应的指令）
    public UnityEvent<string> OnUseSpell = new UnityEvent<string>();



    //////////////////////Boss行为事件////////////////////////////

    //Boss攻击事件
    public UnityEvent<Transform> OnBossAttack = new UnityEvent<Transform>();

    //Boss追逐事件
    public UnityEvent<Transform> OnBossChaseStart = new UnityEvent<Transform>();
    public UnityEvent<Transform> OnBossChaseStop = new UnityEvent<Transform>();

    //Bossy远离事件
    public UnityEvent<Transform> OnBossAwayStart = new UnityEvent<Transform>();
    public UnityEvent<Transform> OnBossAwayStop = new UnityEvent<Transform>();

    //Boss巡逻事件
    public UnityEvent<Transform> OnBossPatrolStart = new UnityEvent<Transform>();
    public UnityEvent<Transform> OnBossPatrolStop = new UnityEvent<Transform>();

}
