using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildWallButtonHandler : MonoBehaviour, Observer
{
    [SerializeField]
    private Globals globals;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        Publisher.AddObserver(this);
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnNotify(PublisherEvent ev)
    {
        switch(ev)
        {
            case PublisherEvent.GameModeChanged:
                OnGameModeChanged();
                break;
            default:
                break;
        }
    }

    public void ToggleBuildWallMode(){
        if (globals.GetGameMode() == GameMode.BuildWall)
        {
            globals.SetGameMode(GameMode.None);
        }
        else {
            globals.SetGameMode(GameMode.BuildWall);
        }
    }

    private void OnGameModeChanged()
    {
        if (globals.GetGameMode() == GameMode.BuildWall)
        {
            text.text = "Stop Build";
            image.color = Color.green;
        }
        else {
            text.text = "Build Wall";
            image.color = Color.white;
        }
    }
}
