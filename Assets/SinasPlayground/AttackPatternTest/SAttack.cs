using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;



[System.Serializable]
public class SAttack : ISAttack
{

    public GameObject  attackAnimationPrefab;
    public float degree;
    private SAttackPrefab attackGO;
    private TaskCompletionSource<bool> currentTask;
    private SAttackTaskSystem _taskSystem;


   public void Initialize(SAttackTaskSystem taskSystem)
    {
        _taskSystem = taskSystem;
    }
    
    public async Task StartAttack() {

        currentTask = new TaskCompletionSource<bool>();
        
        attackGO = _taskSystem.InstantiateAttack(attackAnimationPrefab, degree).GetComponentInChildren<SAttackPrefab>();
        attackGO.OnCompletedAction = () => currentTask.SetResult(true);
        attackGO.PlayAttack();

        await  currentTask.Task;
    }
}

