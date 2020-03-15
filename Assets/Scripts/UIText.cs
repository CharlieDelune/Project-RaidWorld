using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI bitsText;
    [SerializeField]
    private Base baseInfo;
    [SerializeField]
    private Globals globals;

    void Start()
    {
        healthText.text = "Base Health: " + baseInfo.health;
    }


    void Update()
    {
        healthText.text = "Base Health: " + baseInfo.health;
        bitsText.text = "Bits: " + globals.bits;
    }
}
