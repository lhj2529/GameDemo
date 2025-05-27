using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Events/Game Event Channel")]
public class GameEventChannel : ScriptableObject
{
    //传递“连击数”事件
    public UnityEvent<float> OnPassComboStage = new UnityEvent<float>();

    //重置“连击数”事件
    public UnityEvent OnResetCombo = new UnityEvent();

    //攻击命中事件
    public UnityEvent OnAttackHits = new UnityEvent();

    //命中检测事件
    public UnityEvent<float> OnAttackColliderCheck = new UnityEvent<float>(); //传递参数用以选择开启碰撞箱

    //索敌事件(距离角色最近)
    public UnityEvent OnApplicationFind_Nearest=new UnityEvent();
    public UnityEvent<Transform> OnFindNearestEnemy=new UnityEvent<Transform>();
    //索敌事件(距离屏幕中心最近)
    public UnityEvent OnApplicationFind_Center = new UnityEvent();
    public UnityEvent<GameObject> OnFindCenterEnemy = new UnityEvent<GameObject>();
}
