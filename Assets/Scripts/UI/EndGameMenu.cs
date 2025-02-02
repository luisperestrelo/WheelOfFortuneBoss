using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void NewRun()
    {

        RunManager.Instance.RestartGame();


    }

    public void Quit()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public void EndlessMode()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
