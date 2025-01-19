using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorsoraAnimationTriggers : MonoBehaviour
{
    private MorsoraBossController bossController;

    private void Awake()
    {
        bossController = GetComponentInParent<MorsoraBossController>();
    }

    private void AnimationFinishTrigger()
    {
        bossController.AnimationFinishTrigger();
    }
}
