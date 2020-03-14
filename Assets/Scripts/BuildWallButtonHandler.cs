using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class BuildWallButtonHandler : MonoBehaviour
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

    public void ToggleBuildWallMode(){
        if (globals.GetGameMode() == GameMode.BuildWall)
        {
            globals.SetGameMode(GameMode.None);
            text.text = "Build Wall";
        }
        else {
            globals.SetGameMode(GameMode.BuildWall);
            text.text = "Stop Build";
        }
    }
}
