using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreMechanics : MonoBehaviour
{

    private float ComboNum = 0f;
    private float ComboStage = 0f;

    [SerializeField] private GameEventChannel events;

    private GUIStyle labelStyle;

    // Start is called before the first frame update
    void Start()
    {
        events.OnAttackHits.AddListener(ComboAdd);
        events.OnResetCombo.AddListener(ReceiveResetEvent);
    }

    // Update is called once per frame
    void Update()
    {
        switch (ComboNum)
        {
            case 0f:
                events.OnPassComboStage?.Invoke(1);
                ComboStage = 1f;
                break;
            case 20f:
                events.OnPassComboStage?.Invoke(2);
                ComboStage = 2f;
                break;
            case 40f:
                events.OnPassComboStage?.Invoke(3);
                ComboStage = 3f;
                break;
        }
    }

    void ComboAdd()
    {
        ComboNum++;
        Debug.Log("当前连击数为" +ComboNum);
    }

    void ReceiveResetEvent()
    {
        ComboNum = 0f;
    }

    void OnGUI()
    {
        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 35;  // 设置字体大小（单位：像素）

        GUI.Label(new Rect(10, 110, 600, 60), $"当前连击数: {ComboNum}", labelStyle);
        GUI.Label(new Rect(10, 160, 600, 60), $"当前连击阶段: {ComboStage}", labelStyle);

    }
}
