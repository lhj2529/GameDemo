using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJudgment : MonoBehaviour
{
    private bool canAttack = true;

    [SerializeField] private GameEventChannel events;

    //¹¥»÷Åö×²Ïä
    [SerializeField] private GameObject attackCollider;
    private float attackType;
    private Vector3[] attack_Size =new Vector3[]    // Åö×²Ïä³ß´çÊı×é
    {
        //ÆÕÍ¨¹¥»÷ËÄ¶ÎµÄÅö×²Ïä³ß´ç
        new Vector3(1.3845f, 0.9134f, 0.9223f), 
        new Vector3(1.1631f, 0.9134f, 0.9223f),
        new Vector3(1.6687f, 0.9134f, 1.4948f),
        new Vector3(2.1548f, 0.9134f, 1.7252f),

        //½×¶ÎÒ»¼¼ÄÜÅö×²Ïä³ß´ç
        new Vector3(1.9730f, 2.0031f, 1.9608f),

        //½×¶Î¶ş¼¼ÄÜµÄÁ½¶ÎÅö×²Ïä³ß´ç
        new Vector3(2.2955f, 1.2575f, 2.6444f),

        //½×¶ÎÈı¼¼ÄÜµÄÅö×²Ïä
        new Vector3(1.9730f, 1.3176f, 1.9608f),

    }; 

    // Start is called before the first frame update
    void Start()
    {
        attackCollider.GetComponent<BoxCollider>().enabled = false;   //³õÊ¼Åö×²¹Ø±Õ
        events.OnAttackColliderCheck.AddListener(ReceiveCollider);
    }

    private void ReceiveCollider(float attackType_)
    {
        attackType=attackType_-1;
    }

    public void StartAttack()
    {
        if (!canAttack)
            return;
        attackCollider.GetComponent<BoxCollider>().enabled = true;
        attackCollider.GetComponent<BoxCollider>().size = attack_Size[(int)attackType];
        canAttack = false;
    }

    public void EndAttack()
    {
        attackCollider.GetComponent<BoxCollider>().enabled = false;
        canAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("¹¥»÷µ½ÁËµĞÈË");
            events.OnAttackHits?.Invoke();
        }
    }
}
