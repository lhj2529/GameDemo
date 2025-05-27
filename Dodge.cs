using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    // 动画参数常量
    private const string DODGE_DIR_PARAM = "DodgeDirection";
    private const string DODGE_TRIGGER = "Dodge";

    [SerializeField] private float cooldownTime = 1f;   //闪避冷却时间
    [SerializeField] private float directionThreshold = 0.7f;   //闪避阈值

    [SerializeField] private Animator characterAnimator;    //获取动画控制器   

    private bool isDodging=false;   //是否在执行闪避
    private float cooldownTimer = 0f;   //冷却计时器
    private Vector3 movementInput;

    //方向判断
    private enum DodgeDirection { F,B,L,R}

    // Start is called before the first frame update
    void Start()
    {
        if(!characterAnimator)
        {
            characterAnimator = GetComponent<Animator>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCooldown();   
        GetMovementInput();

        if(CanDodge())
        {
            PerformDodge();
        }
    }

    void UpdateCooldown()   //冷却时间重置
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    void GetMovementInput()  //获取输入
    {
        movementInput = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0f,
            Input.GetAxisRaw("Vertical")
            ).normalized;
    }

    bool CanDodge() //闪避条件判断
    {
        return Input.GetKeyDown(KeyCode.LeftShift) &&
            !isDodging &&
            cooldownTimer <= 0;
    }

    private DodgeDirection GetDodgeDirection()     //计算闪避方向
    {
        Vector3 worldDirection = movementInput != Vector3.zero ?
            transform.TransformDirection(movementInput) :
            transform.forward;

        Vector3 localDirection = transform.InverseTransformDirection(worldDirection);
        return DetermineDirection(localDirection);
    }

    private DodgeDirection DetermineDirection(Vector3 localDir)
    {
        //方向判断逻辑
        if(Mathf.Abs(localDir.z)>Mathf.Abs(localDir.x))
        {
            return localDir.z > directionThreshold ?
                DodgeDirection.F :
                DodgeDirection.B;
        }
        else
        {
            return localDir.x > directionThreshold ?
                DodgeDirection.R :
                DodgeDirection.L;
        }
    }

    void PerformDodge()  //闪避
    {
        InitializeDodge();
        var direction = GetDodgeDirection();

        //触发动画
        TriggerDodgeAnimation(direction);

        FinalizeDodge();
    }

    private void TriggerDodgeAnimation(DodgeDirection direction)
    {
        switch(direction)
        {
            case DodgeDirection.F:
                characterAnimator.CrossFade("Dodge_F", 0f);
                break;
            case DodgeDirection.B:
                characterAnimator.CrossFade("Dodge_B", 0f);
                break;
            case DodgeDirection.L:
                characterAnimator.CrossFade("Dodge_L", 0f);
                break;
            case DodgeDirection.R:
                characterAnimator.CrossFade("Dodge_R", 0f);
                break;
        }
    }

    void InitializeDodge()  //闪避功能初始化
    {
        isDodging = true;
        cooldownTimer = cooldownTime;
    }

    void FinalizeDodge()    //闪避结束
    {
        isDodging = false;
    }
}
