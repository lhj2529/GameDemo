using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUseSystem : MonoBehaviour
{
    [SerializeField] private GameEventChannel events;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        events.OnUseSpell.AddListener(SpellUse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpellUse(string spell)
    {
        GameObject spellObj = GetComponent<SpellStoreSystem>().GetPrefab(spell);//»ñÈ¡spell¶ÔÓ¦µÄÔ¤ÖÆÌå
        Instantiate(spellObj, player.transform.position, Quaternion.identity);
    }
}
