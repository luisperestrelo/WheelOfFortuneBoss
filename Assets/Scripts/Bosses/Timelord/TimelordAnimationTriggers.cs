using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelordAnimationTriggers : MonoBehaviour
{
    private TimelordBossController bossController;

    private void Awake()
    {
        bossController = GetComponentInParent<TimelordBossController>();
    }

    private void AnimationFinishTrigger()
    {
        bossController.AnimationFinishTrigger();
    }
} 