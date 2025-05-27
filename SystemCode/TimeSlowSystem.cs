using UnityEngine;

public class TimeSlowSystem : MonoBehaviour
{
    [SerializeField] private float slowdownFactor = 0.5f; // 时间缩放因子
    private float originalFixedDeltaTime;
    [SerializeField] private bool isSlowMotion = false;     // 当前状态
    [SerializeField] private GameObject effect_timeSlow;


    void Start()
    {
        originalFixedDeltaTime = Time.fixedDeltaTime; // 保存初始FixedDeltaTime
    }

    void Update()
    {
        // 检测K键按下
        if (Input.GetKeyDown(KeyCode.K) )
        {

            ToggleSlowMotion();
        }
    }
    public void ToggleSlowMotion()
    {
        isSlowMotion = !isSlowMotion;

        if (isSlowMotion)
        {
            // 实例化预制体
            Instantiate(effect_timeSlow, transform.position, Quaternion.identity);

            // 进入慢动作
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = originalFixedDeltaTime * slowdownFactor;
        }
        else
        {
            // 恢复正常
            Time.timeScale = 1f;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }
    }
}
