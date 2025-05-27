using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpellInputSystem : MonoBehaviour
{
    [SerializeField] private GameEventChannel events;
    public InputField inputField;

    [SerializeField] private GameObject effect_inputting;
    private GameObject instance=null;



    // Start is called before the first frame update
    void Start()
    {
        inputField.onEndEdit.AddListener(SubmitInputString);
    }

    void Update()
    {
        // 按Tab键切换焦点
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inputField.isFocused)
            {
                // 释放输入框焦点
                inputField.DeactivateInputField();
                // 清除EventSystem当前选中对象
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                // 实例化预制体
                instance = Instantiate(effect_inputting, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
                // 设置缩放（例如放大到 2 倍）
                instance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

                inputField.ActivateInputField();
            }
        }

        if (instance != null)
        {
            instance.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }

    private void SubmitInputString(string currentInput)
    {
        events.OnSpellInput?.Invoke(currentInput);//触发指令提交事件

        Destroy(instance, 0.5f);

        // 清空输入框
        inputField.text = "";
        // 释放输入框焦点
        inputField.DeactivateInputField();
        // 清除EventSystem当前选中对象
        EventSystem.current.SetSelectedGameObject(null);
    }
}
