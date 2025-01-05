using UnityEngine;

public class FireFieldsState : BossState
{
    private float stateDuration = 6f;
    private float timeToChangeField = 2f; // Time before changing the field
    private float changeFieldTimer = 0f; // Timer to track time elapsed
    private GameObject[] fields;
    private int currentFireFieldIndex = -1; // Index of the currently active field

    public FireFieldsState(BossStateMachine stateMachine, BossController bossController) : base(stateMachine, bossController)
    {
        fields = bossController.GetFields();
    }

    public override void Enter()
    {
        base.Enter(); // To reset the timer and call Debug.Log
                      // Activate fire fields when entering the state
                      // For example, activate fields 1, 3, and 5:

        //bossController.ActivateFireFields(new int[] { 1, 3, 5 });
        //bossController.ChangeRandomFieldToFire();

        /*      if (fields.Length > 0)
             {
                 int randomIndex = Random.Range(0, fields.Length);
                 bossController.ActivateFireFields(new int[] { randomIndex });
             } */

        currentFireFieldIndex = bossController.ChangeRandomFieldToFire();
        changeFieldTimer = 0f; // Reset the timer

    }

    public override void Update()
    {
        base.Update(); // To update the timer

        UpdateFireFieldTimer();
        // Check if it's time to transition to the next state
        if (timer >= stateDuration)
        {
            stateMachine.ChangeState(new ShockwaveState(stateMachine, bossController));
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
                currentFireFieldIndex = bossController.ChangeRandomFieldToFire(); // Change to a new random field
                changeFieldTimer = 0f; // Reset the timer
            }
        }
    }

}