using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Teleportation : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private float teleportDistance = 3f;      // 基础瞬移距离
    [SerializeField] private float wallDetectionOffset = 0.1f; // 墙体检测偏移量
    [SerializeField] private LayerMask groundLayer;           // 地面层级

    [SerializeField] private GameObject teleportationEffect;

    private Collider2D characterCollider;
    private Vector2[] directions = new Vector2[4];

    void Start()
    {
        characterCollider = GetComponent<Collider2D>();
        InitializeDirections();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            CheckTeleportInput();
        }
    }

    private void InitializeDirections()
    {
        directions[0] = Vector2.up;    // W
        directions[1] = Vector2.down;  // S
        directions[2] = Vector2.left;  // A
        directions[3] = Vector2.right; // D
    }

    private void CheckTeleportInput()
    {
        if (Input.GetKey(KeyCode.W)) PerformTeleport(directions[0]);
        if (Input.GetKey(KeyCode.S)) PerformTeleport(directions[1]);
        if (Input.GetKey(KeyCode.A)) PerformTeleport(directions[2]);
        if (Input.GetKey(KeyCode.D)) PerformTeleport(directions[3]);
    }

    private void PerformTeleport(Vector2 direction)
    {
        Vector2 origin = (Vector2)transform.position + characterCollider.offset;
        float castDistance = teleportDistance + wallDetectionOffset;

        RaycastHit2D hit = Physics2D.BoxCast(
            origin: origin,
            size: characterCollider.bounds.size,
            angle: 0f,
            direction: direction,
            distance: castDistance,
            layerMask: groundLayer
        );

        float actualDistance = CalculateActualDistance(hit, direction);
        ExecuteTeleport(direction, actualDistance);
    }

    private float CalculateActualDistance(RaycastHit2D hit, Vector2 direction)
    {
        if (hit.collider != null && hit.collider.CompareTag("Ground"))
        {
            // 计算安全距离：碰撞距离 - 碰撞体膨胀 - 安全偏移
            return Mathf.Max(0, hit.distance - wallDetectionOffset);
        }
        return teleportDistance;
    }

    private void ExecuteTeleport(Vector2 direction, float distance)
    {
        Vector2 newPosition = (Vector2)transform.position + direction * distance;
        Instantiate(teleportationEffect, newPosition, Quaternion.identity);
        transform.position = newPosition;
    }

    // 可视化调试
    void OnDrawGizmosSelected()
    {
        if (characterCollider == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + (Vector3)characterCollider.offset,
                           characterCollider.bounds.size);
    }
}
