using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpaceBar : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject clickTutorial;
    private bool isTutorialFinished = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTutorialFinished && Input.GetKeyDown(KeyCode.Space))
        {
            isTutorialFinished = true;
            animator.SetBool("isShowing", false);
            clickTutorial.SetActive(true);
            Destroy(gameObject, 1.0f);
        }

    }
}
