using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private Animator animator;
    private static PauseMenu instance;
    
    [SerializeField] private ManualMoveCheckbox manualMoveCheckbox;

    [SerializeField] private AudioClip openSfx;
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (instance != null)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
    public void Pause()
    {
        if (isPaused)
            return;
        Time.timeScale = 0;
        isPaused = true;
        manualMoveCheckbox.UpdateUI();
        animator.SetBool("isPaused", true);
        SFXPool.instance.PlaySound(openSfx, SFXPool.MixGroup.ui);
        
        if(MusicPlayer.instance != null)
            MusicPlayer.instance.SetFilterIntensity(0.6f);
    }

    public void Resume()
    {
        if (!isPaused)
            return;
        Time.timeScale = 1;
        isPaused = false;
        animator.SetBool("isPaused", false);
        if (MusicPlayer.instance != null)
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
