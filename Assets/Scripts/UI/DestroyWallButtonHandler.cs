using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DestroyWallButtonHandler : MonoBehaviour, Observer
{
    [SerializeField]
    private Globals globals;
    [SerializeField]
    private TextMeshProUGUI text;
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

    public void ToggleDestroyWallMode(){
        if (globals.GetGameMode() == GameMode.DestroyWall)
        {
            globals.SetGameMode(GameMode.None);
        }
        else {
            globals.SetGameMode(GameMode.DestroyWall);
        }
    }

    private void OnGameModeChanged()
    {
        if (globals.GetGameMode() == GameMode.DestroyWall)
        {
            text.text = "Stop Destroy";
            image.color = Color.green;
        }
        else {
            text.text = "Destroy Wall";
            image.color = Color.white;
        }
    }
}
