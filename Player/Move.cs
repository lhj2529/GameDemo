using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Jump Settings")]
    public int maxJumps = 2;        // 最大跳跃次数
    private int currentJumps = 0;   // 当前跳跃计数
    private bool wasGrounded;       // 上一帧的接地状态

    private Rigidbody2D rb;
    private bool isGrounded;

    private Vector3 originalScale;      // 初始缩放值

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // 保存初始缩放值
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 地面检测
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // 新增：落地时重置跳跃计数
        if (isGrounded && !wasGrounded)
        {
            currentJumps = 0;
        }
        wasGrounded = isGrounded;

        // 修改后的跳跃输入检测
        if (Input.GetKeyDown(KeyCode.Space) && currentJumps < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            currentJumps++;
        }
    }

    void FixedUpdate()
    {
        // 水平移动
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 新增方向控制逻辑
        if ( moveInput != 0)
        {
            // 根据输入方向翻转角色
            Vector3 newScale = originalScale;
            newScale.x = Mathf.Sign(moveInput) * originalScale.x;
            transform.localScale = newScale;
        }
    }

    // 可视化地面检测范围
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
