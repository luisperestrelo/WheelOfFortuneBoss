using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


[System.Serializable]
public class SAttackPattern :  ISAttack
{
    [SerializeField]
    private List<SAttackPattern> attackPatterns;
    [SerializeField]
    public List<SAttack> singleAttacks;
    [SerializeField]
    private float _idleTimeAfter;
    [SerializeField]
    bool _isParallel;

    private List<ISAttack> attacks;
    // public SAttackPattern(List<SAttackPattern> attackPatterns, bool isParallel = false, float idleTimeAfter = 1f)
    // {

    //     _idleTimeAfter = idleTimeAfter;
    //     _isParallel = isParallel;
    // }


    public void Initialize(SAttackTaskSystem taskSystem){
        attackPatterns.ForEach(attack => attack.Initialize(taskSystem));
        singleAttacks.ForEach(attack => attack.Initialize(taskSystem));
    }

    private void CombineAttacks()
    {
        attacks = new List<ISAttack>();

        // Add all attack patterns
        if (attackPatterns != null)
        {
            attacks.AddRange(attackPatterns);
        }

        // Add all single attacks
        if (singleAttacks != null)
        {
            attacks.AddRange(singleAttacks);
        }
    }

    public async Task StartAttack()
    {
        CombineAttacks();

        if (_isParallel)
        {
            await StartAttackInParallel();
        }
        else
        {
            await StartAttackInSequence();
        }
        await Task.Delay((int)(_idleTimeAfter * 1000));

    }

    public async Task StartAttackInSequence()
    {
        foreach (var attack in attacks)
        {
            await attack.StartAttack();
        }


    }

    public async Task StartAttackInParallel()
    {
        var attackTasks = new List<Task>();
        foreach (var attack in attacks)
        {
            attackTasks.Add(attack.StartAttack());
        }

        await Task.WhenAll(attackTasks);

    }


}
