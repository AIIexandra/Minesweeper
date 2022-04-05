using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject panelPause;

    public void ContinueGame()
    {
        MainScript.timeRun = true;
        panelPause.SetActive(false);
    }

    public void OpenPanelPause()
    {
        MainScript.timeRun = false;
        panelPause.SetActive(true);
    }
}
