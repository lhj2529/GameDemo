using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // 移动速度
    public float gravity = -9.81f;    // 重力加速度
    public float jumpHeight = 2f;     // 跳跃高度

    private Animator animator;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    private bool canMove = true;
    private Vector2 lastInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator=GetComponent<Animator>();
    }

    void Update()
    {
        // 检测是否接触地面
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 轻微下压防止悬空
        }

        if (canMove)
        {
            // 获取输入
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            

            // 归一化输入向量，防止斜向移动速度过快
            Vector2 input = new Vector2(horizontal, vertical).normalized;


            if (horizontal != 0 || vertical != 0)
            {
                animator.SetBool("isMove", true);
                animator.SetFloat("Horizontal", input.x);
                animator.SetFloat("Vertical", input.y);
                lastInput = new Vector2(input.x, input.y);

            }
            else if (horizontal == 0 && vertical == 0)
            {
                animator.SetFloat("Horizontal", lastInput.x);
                animator.SetFloat("Vertical", lastInput.y);
                animator.SetBool("isMove", false);
            }

            // 根据输入计算移动方向（世界坐标系）
            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

            // 应用移动（已考虑帧率）
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }

        // 跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 应用重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void CanMove()
    {
        canMove = true;
    }
    public void stopMove()
    {
        canMove = false;
    }
}
