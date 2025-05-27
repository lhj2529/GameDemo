using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    //单例实例
    public static EffectsManager Instance { get; private set; }

    //池配置：存储预制体
    [System.Serializable]
    public class Pool
    {
        public string tag;  //对象标识
        public GameObject prefab;   //预制体
    }

    public List<Pool> pools;    //所有对象池配置
    private Dictionary<string, Queue<GameObject>> poolDictionary;   //实际对象池

    private void Awake()
    {
        //单例初始化
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //初始化所有对象池
        InitializePools();
    }

    //初始化所有池
    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool=new Queue<GameObject>();
            poolDictionary.Add(pool.tag, objectPool);
        }

    }

    // 创建新对象并设置为未激活
    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        obj.transform.SetParent(transform); // 可选：将对象设为池的子物体
        return obj;
    }

    public GameObject GetFromPool(string tag,Vector3 position,Quaternion rotation)
    {
        if(!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("对象池未找到");
            return null;
        }

        Queue<GameObject> poolQueue=poolDictionary[tag];

        //如果池为空，动态创建一个新对象
        if (poolQueue.Count == 0)
        {
            Pool poolConfig = pools.Find(p => p.tag == tag);
            if(poolConfig!=null)
            {
                GameObject newObj=CreateNewObject(poolConfig.prefab);
                poolQueue.Enqueue(newObj);
            }
            else
            {
                Debug.Log("未找到标签的池");
                return null;
            }
        }

        GameObject obj= poolQueue.Dequeue();
        obj.transform.position = position + obj.transform.position;

        //if(rotation== Quaternion.identity)
        //    obj.transform.rotation = obj.transform.rotation;
        //else
        //    obj.transform.rotation = rotation;
        obj.SetActive(true);

        return obj;
    }

    //回收到对象池
    public void ReturnToPool(string tag,GameObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("未找到标签");
            return;
        }

        //重置对象状态
        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}
