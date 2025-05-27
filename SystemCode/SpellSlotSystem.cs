using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlotSystem : MonoBehaviour
{
    [SerializeField] private GameEventChannel events;
    [SerializeField] private Transform slotPanel; //技能槽容器

    // Start is called before the first frame update
    void Start()
    {
        events.OnStoreSpell.AddListener(SpellStoreToSlot);//监听存储事件
    }

    // Update is called once per frame
    void Update()
    {
        SkillKeyDetection();
    }

    private void SpellStoreToSlot(string command)
    {
        GameObject spellObj = GetComponent<SpellStoreSystem>().GetPrefab(command);//获取command对应的预制体
        for(int i = 0; i < slotPanel.childCount; i++)
        {
            if (!slotPanel.GetChild(i).GetComponent<SlotUI>().isOccupied)
            {
                slotPanel.GetChild(i).GetComponent<SlotUI>().ChangeSlotState(command,spellObj);
                break;
            }
        }
    }

    private void SkillKeyDetection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (slotPanel.GetChild(0).GetComponent<SlotUI>().isOccupied)
            {
                string spell = slotPanel.GetChild(0).GetComponent<SlotUI>().ReturnSpell(); //读取槽1存储的法术名
                events.OnUseSpell?.Invoke(spell); //触发法术释放事件
                slotPanel.GetChild(0).GetComponent<SlotUI>().SlotReset(); //还原槽1为空
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (slotPanel.GetChild(1).GetComponent<SlotUI>().isOccupied)
            {
                string spell = slotPanel.GetChild(1).GetComponent<SlotUI>().ReturnSpell(); //读取槽2存储的法术名
                events.OnUseSpell?.Invoke(spell); //触发法术释放事件
                slotPanel.GetChild(1).GetComponent<SlotUI>().SlotReset(); //还原槽2为空
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (slotPanel.GetChild(2).GetComponent<SlotUI>().isOccupied)
            {
                string spell = slotPanel.GetChild(2).GetComponent<SlotUI>().ReturnSpell(); //读取槽3存储的法术名
                events.OnUseSpell?.Invoke(spell); //触发法术释放事件
                slotPanel.GetChild(2).GetComponent<SlotUI>().SlotReset(); //还原槽3为空
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (slotPanel.GetChild(3).GetComponent<SlotUI>().isOccupied)
            {
                string spell = slotPanel.GetChild(3).GetComponent<SlotUI>().ReturnSpell(); //读取槽4存储的法术名
                events.OnUseSpell?.Invoke(spell); //触发法术释放事件
                slotPanel.GetChild(3).GetComponent<SlotUI>().SlotReset(); //还原槽4为空
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (slotPanel.GetChild(4).GetComponent<SlotUI>().isOccupied)
            {
                string spell = slotPanel.GetChild(4).GetComponent<SlotUI>().ReturnSpell(); //读取槽5存储的法术名
                events.OnUseSpell?.Invoke(spell); //触发法术释放事件
                slotPanel.GetChild(4).GetComponent<SlotUI>().SlotReset(); //还原槽5为空
            }
        }
    }
}
