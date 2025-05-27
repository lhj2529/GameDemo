using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCheckSystem : MonoBehaviour
{
    [SerializeField] private GameEventChannel events;
    [SerializeField] private GameObject effect_inputRight;

    // Start is called before the first frame update
    void Start()
    {
        events.OnSpellInput.AddListener(SpellCheck);//监听输入事件
    }

    private void SpellCheck(string input)
    {
        input = input.ToUpper().Trim();//转为大写并去除字符串的空格
        bool isRight= GetComponent<SpellStoreSystem>().SearchInDictionary(input);//在字典中查找“input”是否在指令字典中

        if (isRight)
        {
            GameObject instance = Instantiate(effect_inputRight, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
            // 设置缩放（例如放大到 2 倍）
            instance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            events.OnStoreSpell?.Invoke(input);
        }
        else
        {
            Debug.Log("输入无效");
        }
    }
}
