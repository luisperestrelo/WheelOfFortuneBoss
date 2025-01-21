using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialClick : MonoBehaviour
{
    private Animator animator;
    private bool isTutorialFinished = false;
    [SerializeField] private GameObject slowdownTutorial;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTutorialFinished && Input.GetMouseButton(0))
        {
            isTutorialFinished = true;
            animator.SetBool("isShowing", false);
            slowdownTutorial.SetActive(true);
            Destroy(gameObject, 1.0f);
            Destroy(slowdownTutorial, 3.0f);
        }

    }
}
