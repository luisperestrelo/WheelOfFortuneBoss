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

    private BossController bossController;

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
} 