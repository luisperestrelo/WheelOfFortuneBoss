using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MorsoraAnimationHandler : MonoBehaviour
{

    [SerializeField] 
    private Animator scytheDarkAnimator;
    [SerializeField] 
    private Animator scytheLightAnimator;



  public void ScytheSlashDark() {
    scytheDarkAnimator.SetTrigger("Attack");
  }

   public void ScytheSlashLight() {
    scytheLightAnimator.SetTrigger("Attack");
  }
}
