using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f; // 鼠标灵敏度
    public Transform playerBody;         // 角色身体（用于左右旋转）
    public Transform cameraTransform;    // 相机（用于上下旋转）

    private float xRotation = 0f;

    void Start()
    {
        // 锁定并隐藏鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // 获取鼠标输入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 控制上下视角（限制-90°~90°）
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 应用相机上下旋转
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 应用角色左右旋转
        playerBody.Rotate(Vector3.up * mouseX);
    }
}