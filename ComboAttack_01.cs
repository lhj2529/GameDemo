using UnityEngine;

public class ComboAttack_01 : MonoBehaviour
{
    [Header("连击设置")]
    public int maxCombo = 4;             // 最大连击数
    public float comboResetTime = 2f;    // 连击重置时间
    public float[] attackIntervals;      // 每段攻击之间的间隔

    private int currentCombo = 0;        // 当前连击段数
    private float lastAttackTime;        // 最后攻击时间
    private bool isAttacking = false;    // 是否正在攻击中

    private GameObject effect;
    [SerializeField] private Animator animator1;

    [SerializeField] private Transform effectPos;

    [SerializeField] private GameEventChannel events;

    private GUIStyle labelStyle;

    void Start()
    {

    }

    void Update()
    {
        // 检测攻击输入
        if (Input.GetMouseButtonDown(0) && CanAttack())
        {
            StartCoroutine(ExecuteAttack());
        }

        // 连击超时检测
        if (Time.time - lastAttackTime > comboResetTime && currentCombo > 0)
        {
            ResetCombo();
        }
    }

    bool CanAttack()
    {
        // 当前不在攻击状态且连击未超过上限
        return !isAttacking && currentCombo < maxCombo;
    }

    System.Collections.IEnumerator ExecuteAttack()
    {
        isAttacking = true;
        currentCombo++;
        lastAttackTime = Time.time;

        // 执行当前段攻击
        switch (currentCombo)
        {
            case 1:
                //Debug.Log("第一段攻击");
                events.OnAttackColliderCheck?.Invoke(1);
                animator1.CrossFade("combo_attack_01_01", 0.008f);
                effect= EffectsManager.Instance.GetFromPool(
                        "Combo_Attack_01",
                        effectPos.position,
                        Quaternion.identity
                         ) ;
                
                break;
            case 2:
                //Debug.Log("第二段攻击");
                events.OnAttackColliderCheck?.Invoke(2);
                animator1.CrossFade("combo_attack_01_02", 0f);
                EffectsManager.Instance.GetFromPool(
                        "Combo_Attack_02",
                        effectPos.position,
                        Quaternion.identity
                         );
                EffectsManager.Instance.GetFromPool(
                        "Combo_Attack_02_01",
                        effectPos.position,
                        Quaternion.identity
                         );
                break;
            case 3:
                //Debug.Log("第三段攻击");
                events.OnAttackColliderCheck?.Invoke(3);
                animator1.CrossFade("combo_attack_01_03",  0f);
                EffectsManager.Instance.GetFromPool(
                        "Combo_Attack_03",
                        effectPos.position,
                        Quaternion.identity
                         );
                break;
            case 4:
                //Debug.Log("第四段攻击");
                events.OnAttackColliderCheck?.Invoke(4);
                animator1.CrossFade("combo_attack_01_04", 0f);
                EffectsManager.Instance.GetFromPool(
                        "Combo_Attack_04",
                        effectPos.position,
                        Quaternion.identity
                         );
                EffectsManager.Instance.GetFromPool(
                        "Combo_Attack_04_01",
                        effectPos.position,
                        Quaternion.identity
                         );
                break;
        }

        // 等待攻击间隔
        float interval = attackIntervals[Mathf.Clamp(currentCombo - 1, 0, attackIntervals.Length - 1)];
        yield return new WaitForSeconds(interval);

        // 自动重置连击
        if (currentCombo >= maxCombo)
        {
            ResetCombo();
        }
        else
        {
            isAttacking = false;
        }
    }

    void ResetCombo()
    {
        currentCombo = 0;
        isAttacking = false;   
    }

    // 可视化调试信息
    void OnGUI()
    {
        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 35;  // 设置字体大小（单位：像素）

        GUI.Label(new Rect(10, 10, 600, 60), $"当前普攻连击段数: {currentCombo}", labelStyle);
        GUI.Label(new Rect(10, 60, 600, 60), $"普攻剩余重置时间: {comboResetTime - (Time.time - lastAttackTime):F1}", labelStyle);
    }
}
