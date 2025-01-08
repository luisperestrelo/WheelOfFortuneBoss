using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    public BossState currentState { get; private set; }

    // States
    public IdleState idleState;
    public FireFieldsState fireFieldsState;
    public ShockwaveState shockwaveState;
    public GhostSummonState ghostSummonState;
    public SpearAndShockwaveState spearAndShockwaveState;
    public LaserSweepState laserSweepState;
    public MinionSummonState minionSummonState;
    public ThrowSpearsState throwSpearsState;
    public ExplosionsState explosionsState;
    public RandomShockwaveAndAbilityState randomShockwaveAndAbilityState;
    public IncapacitatedState incapacitatedState;

    private BossController bossController;
    private BossState nextStateAfterIncapacitated;
    private float remainingStateTime;
    private Queue<BossState> stateTransitionQueue = new Queue<BossState>();

    public BossStateMachine(BossController bossController)
    {
        this.bossController = bossController;

        // Initialize all states here
        idleState = new IdleState(this, bossController);
        fireFieldsState = new FireFieldsState(this, bossController);
        shockwaveState = new ShockwaveState(this, bossController);
        ghostSummonState = new GhostSummonState(this, bossController);
        spearAndShockwaveState = new SpearAndShockwaveState(this, bossController);
        laserSweepState = new LaserSweepState(this, bossController);
        minionSummonState = new MinionSummonState(this, bossController);
        throwSpearsState = new ThrowSpearsState(this, bossController);
        explosionsState = new ExplosionsState(this, bossController);
        randomShockwaveAndAbilityState = new RandomShockwaveAndAbilityState(this, bossController);
        incapacitatedState = new IncapacitatedState(this, bossController, 5f);
    }

    public void Initialize(BossState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(BossState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public void TriggerIncapacitatedState(BossState nextState, float duration)
    {
        nextStateAfterIncapacitated = nextState;
        remainingStateTime = nextState.GetTimer();
        incapacitatedState.SetDuration(duration);

        // Request the state change instead of directly changing it
        RequestStateChange(incapacitatedState);
    }

    public void TransitionToNextStateAfterIncapacitated()
    {
        ResumeState(nextStateAfterIncapacitated, remainingStateTime);
    }

    public void ResumeState(BossState state, float time)
    {
        currentState.Exit();
        currentState = state;

        currentState.ResumeState(time);
    }

    public void Update()
    {
        ProcessStateTransitionQueue();
        currentState.Update();
    }

    private void ProcessStateTransitionQueue()
    {
        if (stateTransitionQueue.Count > 0)
        {
            BossState nextState = stateTransitionQueue.Dequeue();
            ChangeState(nextState);
        }
    }

    //In most cases, we want to request a state change instead of directly changing it so that we don't have race conditions
    public void RequestStateChange(BossState newState)
    {
        stateTransitionQueue.Enqueue(newState);
    }
} 