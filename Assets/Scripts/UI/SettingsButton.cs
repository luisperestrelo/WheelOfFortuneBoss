using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    public void PauseProxy()
    {
        pauseMenu.Pause();
    }
}
