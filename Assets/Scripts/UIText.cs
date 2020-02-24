using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private Base baseInfo;

    void Start()
    {
        textMesh.text = "Base Health: " + baseInfo.health;
    }


    void Update()
    {
        textMesh.text = "Base Health: " + baseInfo.health;
    }
}
