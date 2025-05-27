using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    public bool isOccupied=false; //是否被占据标识
    private string storedSpell = ""; //存储的spell字段
    [SerializeField] private GameObject spellIcon;
   
    [SerializeField]private enum SlotNum
    {
        one,
        two,
        three,
        four,
        five,
    }
    [SerializeField] private SlotNum slot;

    public void ChangeSlotState(string spell,GameObject spellObj) 
    {
        this.isOccupied=true;
        this.storedSpell = spell;
        this.spellIcon.GetComponent<Image>().sprite = spellObj.GetComponent<Image>().sprite;
        this.spellIcon.SetActive(true);
    }

    public void SlotReset()
    {
        this.isOccupied=false;
        this.storedSpell = "";
        this.spellIcon.SetActive(false);
    }

    public string ReturnSpell()
    {
        return this.storedSpell;
    }
}
