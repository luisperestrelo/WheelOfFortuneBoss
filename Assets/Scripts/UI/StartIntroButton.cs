using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartIntroButton : MonoBehaviour
{
    [SerializeField] private Animator titleAnimator;
    [SerializeField] private DialogueAnimator dialogueAnimator;
    [SerializeField] private AudioClip clickSfx;
    private bool isPressed = false;
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void StartIntro()
    {
        if (isPressed)
            return; //theoretically shouldn't be a problem regardless, but just in case...
        isPressed = true;

        titleAnimator.SetBool("isShowing", false);
        dialogueAnimator.StartCoroutine(dialogueAnimator.CutsceneRoutine());

        source.PlayOneShot(clickSfx);
    }

}
