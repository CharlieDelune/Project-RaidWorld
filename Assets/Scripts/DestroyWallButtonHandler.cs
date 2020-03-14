using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class DestroyWallButtonHandler : MonoBehaviour
{
    [SerializeField]
    private Globals globals;
    [SerializeField]
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToggleDestroyWallMode(){
        if (globals.GetGameMode() == GameMode.DestroyWall)
        {
            globals.SetGameMode(GameMode.None);
            text.text = "Destroy Wall";
        }
        else {
            globals.SetGameMode(GameMode.DestroyWall);
            text.text = "Stop Destroy";
        }
    }
}
