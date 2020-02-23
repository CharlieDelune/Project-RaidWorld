using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private bool paused;
    [SerializeField]
    private TextMeshProUGUI text;
    
    void Start()
    {
        paused = false;
    }
    public void PlayPause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            text.text = "Pause";
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            text.text = "Play";
            paused = true;
        }
    }
}
