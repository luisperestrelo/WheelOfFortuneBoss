using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private Animator animator;
    private PauseMenu instance;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Pause()
    {
        if (isPaused)
            return;
        Time.timeScale = 0;
        isPaused = true;
        animator.SetBool("isPaused", true);
        MusicPlayer.instance.SetFilterIntensity(0.8f);
    }

    public void Resume()
    {
        if (!isPaused)
            return;
        Time.timeScale = 1;
        isPaused = false;
        animator.SetBool("isPaused", false);
        MusicPlayer.instance.SetFilterIntensity(0f); //This will cause an audio bug if the player unpauses while in the upgrade orb screen.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                Pause();
            else
                Resume();
        }

    }
}
