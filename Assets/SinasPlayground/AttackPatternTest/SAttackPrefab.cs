using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SAttackPrefab : MonoBehaviour
{

    public  System.Action OnCompletedAction;

    private Animator animator;
    // Start is called before the first frame update

    void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }   

    public void OnCompleted()
    {
       OnCompletedAction?.Invoke();
    }

}
