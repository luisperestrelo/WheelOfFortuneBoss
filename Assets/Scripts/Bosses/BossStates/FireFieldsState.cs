using UnityEngine;

public class FireFieldsState : BossState
{
    private float stateDuration = 6f;
    private float timeToChangeField = 2f; 
    private float changeFieldTimer = 0f; 
    private GameObject[] fields;
    private int currentFireFieldIndex = -1; 

    public FireFieldsState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        fields = bossController.GetFields();
    }

    public override void Enter()
    {
        base.Enter(); 

        //bossController.ActivateFireFields(new int[] { 1, 3, 5 });
        //bossController.ChangeRandomFieldToFire();

        /*      if (fields.Length > 0)
             {
                 int randomIndex = Random.Range(0, fields.Length);
                 bossController.ActivateFireFields(new int[] { randomIndex });
             } */

        currentFireFieldIndex = bossController.ChangeRandomFieldToFire();
        changeFieldTimer = 0f; 

    }

    public override void Update()
    {
        base.Update(); 

        UpdateFireFieldTimer();
        if (timer >= stateDuration)
        {
            stateMachine.RequestStateChange(stateMachine.shockwaveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        bossController.DeactivateFireFields();
    }

    private void UpdateFireFieldTimer()
    {
        if (currentFireFieldIndex != -1) // Only update if a field is on fire
        {
            changeFieldTimer += Time.deltaTime;

            if (changeFieldTimer >= timeToChangeField)
            {
                bossController.DeactivateFireFields();
                currentFireFieldIndex = bossController.ChangeRandomFieldToFire(); // Change to a new random field. ATM it can change
                // to the same field, but we can fix that later 
                changeFieldTimer = 0f; 
            }
        }
    }

}