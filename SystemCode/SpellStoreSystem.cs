using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CommandPrefabPair
{
    public string command;      // 指令字符串（例如"create_cube"）
    public GameObject prefab;   // 对应的预制体
}

public class SpellStoreSystem : MonoBehaviour
{
    // Inspector可见的配置列表
    [SerializeField]
    private List<CommandPrefabPair> commandPrefabList = new List<CommandPrefabPair>();

    // 实际使用的字典
    private Dictionary<string, GameObject> commandDictionary = new Dictionary<string, GameObject>();

    void Start()
    {
        // 初始化字典
        foreach (var pair in commandPrefabList)
        {
            // 防御性编程：跳过无效条目
            if (string.IsNullOrEmpty(pair.command) || pair.prefab == null)
            {
                Debug.LogError($"无效配置: {pair.command}");
                continue;
            }

            // 防止重复键
            if (commandDictionary.ContainsKey(pair.command))
            {
                Debug.LogError($"重复指令: {pair.command}");
                continue;
            }

            commandDictionary.Add(pair.command.ToUpper(), pair.prefab);
        }
    }


    // 根据指令获取预制体
    public GameObject GetPrefab(string command)
    {
        if (commandDictionary.TryGetValue(command, out GameObject prefab))
        {
            return prefab;
        }
        Debug.LogWarning($"未知指令: {command}");
        return null;
    }

    public bool SearchInDictionary(string input)
    {
        return commandDictionary.ContainsKey(input);
    }
}

