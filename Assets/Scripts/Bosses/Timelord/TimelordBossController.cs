using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimelordBossController : MonoBehaviour
{
    [SerializeField] private float bossHealthUpgradeInterval = 0.1f;

    public Animator anim { get; private set; }
    public TimelordBossStateMachine stateMachine { get; private set; }

    public FlailAttack flailAttack;
    public StartBigWaveOfDoom startBigWaveOfDoom;
    public MeteorSpawner meteorSpawner;
    public ClockMobSpawner clockMobSpawner;
    public ControlPortals controlPortals;
    public SpawnRealityRend spawnRealityRend;
    public PredictTheFutureAbility predictTheFutureAbility;
    public SpawnTimeRift spawnTimeRift;
    public BlackholeSpawner blackholeSpawner;
    public EnableRotatingOrbs enableRotatingOrbs;


    public TimelordIdleState idleState { get; private set; }

    public TimelordBossState placeholderState { get; private set; }

    public TimelordMeteorShowerState meteorShowerState { get; private set; }
    public TimelordMultiFlailAttackState multiFlailAttackState { get; private set; }
    public TimelordBigWaveOfDoomState bigWaveOfDoomState { get; private set; }
    public TimelordRealityRendState realityRendState { get; private set; }
    public TimelordPredictTheFutureState predictTheFutureState { get; private set; }
    public TimeLordRotatingOrbsState rotatingOrbsState { get; private set; }
    public TimelordTakeABreakState takeABreakState { get; private set; }

    private Dictionary<string, Coroutine> constantAbilityCoroutines = new Dictionary<string, Coroutine>();
    private Dictionary<string, bool> constantAbilityEnabled = new Dictionary<string, bool>();

    public int basicAttackCounter = 0;
    private int basicAttacksPerCycle = 6; // 1 more than how many basic attacks there are, cos we increment first   
    private List<TimelordBossState> availableSpecialAttacksPhase1;
    private List<TimelordBossState> availableSpecialAttacksPhase2;
    private List<TimelordBossState> availableSpecialAttacks;

    private float chanceAdjustment = 0.2f;

    public List<TimelordBossState> basicAttackStatesPhase1;
    public List<TimelordBossState> basicAttackStatesPhase2;
    public List<TimelordBossState> basicAttackStates;
    public Dictionary<TimelordBossState, float> basicAttackProbabilities;

    private BossHealth health;
    [Range(0, 1)]
    [SerializeField] private List<float> upgradeOrbSpawnHealthThresholds = new List<float>();
    [SerializeField] private float bossPhase2HealthThreshold = -1f; // no p2 but we keep the code
    private bool isPhase2 = false;
    public bool shouldTransitionToPhase2 { get; private set; } = false;

    [SerializeField] private GameObject upgradeOrb;
    [SerializeField] public Transform player { get; private set; }
    [SerializeField] private CircularPath playerPath;

    public Hourglass hourglass;

    public WheelManager wheelManager { get; private set; }




    private void Awake()
    {
        wheelManager = FindObjectOfType<WheelManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerPath = FindObjectOfType<CircularPath>();

        flailAttack = GetComponent<FlailAttack>();
        startBigWaveOfDoom = GetComponent<StartBigWaveOfDoom>();
        meteorSpawner = GetComponent<MeteorSpawner>();
        clockMobSpawner = GetComponent<ClockMobSpawner>();
        controlPortals = GetComponent<ControlPortals>();
        spawnRealityRend = GetComponent<SpawnRealityRend>();
        predictTheFutureAbility = GetComponent<PredictTheFutureAbility>();
        spawnTimeRift = GetComponent<SpawnTimeRift>();
        blackholeSpawner = GetComponent<BlackholeSpawner>();
        enableRotatingOrbs = GetComponent<EnableRotatingOrbs>();


        stateMachine = new TimelordBossStateMachine();
        idleState = new TimelordIdleState(stateMachine, this, "Idle");

        placeholderState = new TimelordBossState(stateMachine, this, "Idle");

        meteorShowerState = new TimelordMeteorShowerState(stateMachine, this);
        multiFlailAttackState = new TimelordMultiFlailAttackState(stateMachine, this);
        bigWaveOfDoomState = new TimelordBigWaveOfDoomState(stateMachine, this);
        realityRendState = new TimelordRealityRendState(stateMachine, this);
        predictTheFutureState = new TimelordPredictTheFutureState(stateMachine, this);
        takeABreakState = new TimelordTakeABreakState(stateMachine, this, 3f);
        rotatingOrbsState = new TimeLordRotatingOrbsState(stateMachine, this);

        // Initialize basic attack states
        basicAttackStatesPhase1 = new List<TimelordBossState>
        {
            meteorShowerState,
            multiFlailAttackState,
            rotatingOrbsState
        };


        basicAttackStatesPhase2 = new List<TimelordBossState>
        {
            meteorShowerState,
            multiFlailAttackState,
            rotatingOrbsState
        };


        basicAttackStates = basicAttackStatesPhase1;

        health = GetComponent<BossHealth>();
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();

        InitializeBasicAttackProbabilities();
        InitializeConstantAbility("spawnClockMob", 16f);
        InitializeConstantAbility("spawnBlackhole", 5f);
        InitializeConstantAbility("spawnTimeRift", 6f);

        //stateMachine.Initialize(idleState);
        stateMachine.Initialize(takeABreakState);

        InitializeSpecialAttacksPhase1();
        InitializeSpecialAttacksPhase2();
        availableSpecialAttacks = availableSpecialAttacksPhase1;

        enableRotatingOrbs.DisableOrbHolder();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        CheckIfUpgradeThreshold();
        CheckPhase2Transition();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L)) // for testing
        {
            //stateMachine.ChangeState(shockwavesState);
            stateMachine.ChangeState(bigWaveOfDoomState);
        }
#endif




    }

    private void CheckPhase2Transition()
    {
        if (!isPhase2 && health.GetCurrentHealth() <= health.GetMaxHealth() * bossPhase2HealthThreshold && !shouldTransitionToPhase2)
        {
            shouldTransitionToPhase2 = true;
        }
    }

    private void CheckIfUpgradeThreshold()
    {
        if (upgradeOrbSpawnHealthThresholds.Count == 0) return;

        float currentThreshold = upgradeOrbSpawnHealthThresholds[0];

        if (health.GetCurrentHealth() <= health.GetMaxHealth() * currentThreshold)
        {
            SpawnUpgradeOrb();
            upgradeOrbSpawnHealthThresholds.RemoveAt(0);
        }
    }

    public void SpawnUpgradeOrb()
    {
        Vector3 toPlayer = player.position - playerPath.GetCenter();
        float currentAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        currentAngle += 180f;

        float x = playerPath.GetCenter().x + playerPath.GetRadius() * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = playerPath.GetCenter().y + playerPath.GetRadius() * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 spawnPosition = new Vector3(x, y, transform.position.z);

        Instantiate(upgradeOrb, spawnPosition, Quaternion.identity);
    }

    public void SpawnUpgradeOrbWithOffset(float offset)
    {
        Vector3 toPlayer = player.position - playerPath.GetCenter();
        float currentAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        currentAngle += 180f + offset;

        float x = playerPath.GetCenter().x + playerPath.GetRadius() * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = playerPath.GetCenter().y + playerPath.GetRadius() * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 spawnPosition = new Vector3(x, y, transform.position.z);

        Instantiate(upgradeOrb, spawnPosition, Quaternion.identity);
    }

    private void InitializeSpecialAttacksPhase1()
    {
        availableSpecialAttacksPhase1 = new List<TimelordBossState>
        {
            bigWaveOfDoomState,
            realityRendState,
            predictTheFutureState
        };
    }

    private void InitializeSpecialAttacksPhase2()
    {
        availableSpecialAttacksPhase2 = new List<TimelordBossState>
        {
            bigWaveOfDoomState,
            realityRendState,
            predictTheFutureState
        };
    }

    private void InitializeConstantAbility(string abilityName, float interval)
    {
        constantAbilityEnabled[abilityName] = true;
        constantAbilityCoroutines[abilityName] = StartCoroutine(RunConstantAbility(abilityName, interval));
    }

    private IEnumerator RunConstantAbility(string abilityName, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            if (constantAbilityEnabled[abilityName])
            {
                switch (abilityName)
                {
                    case "spawnClockMob":
                        clockMobSpawner.SpawnClockMob();
                        break;
                    case "spawnBlackhole":
                        blackholeSpawner.SpawnBlackhole();
                        break;
                    case "spawnTimeRift":
                        spawnTimeRift.SpawnTimeRiftAtRandomAngle();
                        break;
                    default:
                        Debug.LogError("Unknown constant ability: " + abilityName);
                        break;
                }
            }
        }
    }

    public void SetConstantAbilityStatus(string abilityName, bool isEnabled)
    {
        if (constantAbilityCoroutines.ContainsKey(abilityName))
        {
            constantAbilityEnabled[abilityName] = isEnabled;
        }
        else
        {
            Debug.LogWarning("No constant ability found with name: " + abilityName);
        }
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public TimelordBossState GetRandomState()
    {
        List<TimelordBossState> availableStates = new List<TimelordBossState>
        {
            meteorShowerState,
            multiFlailAttackState,
            bigWaveOfDoomState,
            realityRendState,
            predictTheFutureState

        };

        int randomIndex = Random.Range(0, availableStates.Count);
        return availableStates[randomIndex];
    }

    public void DisableAllConstantAbilities()
    {
        List<string> abilityNames = new List<string>(constantAbilityEnabled.Keys);

        foreach (string abilityName in abilityNames)
        {
            SetConstantAbilityStatus(abilityName, false);
        }
    }

    public void RestartAllConstantAbilities()
    {
        List<string> abilityNames = new List<string>(constantAbilityEnabled.Keys);

        foreach (string abilityName in abilityNames)
        {
            SetConstantAbilityStatus(abilityName, true);
        }
    }

    public TimelordBossState GetRandomBasicAttackState()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;
        TimelordBossState chosenState = null;

        foreach (KeyValuePair<TimelordBossState, float> entry in basicAttackProbabilities)
        {
            cumulativeProbability += entry.Value;
            if (randomValue <= cumulativeProbability)
            {
                chosenState = entry.Key;
                break;
            }
        }

        float probabilityToDistribute = basicAttackProbabilities[chosenState] * chanceAdjustment;
        basicAttackProbabilities[chosenState] -= probabilityToDistribute;

        float probabilityToAdd = probabilityToDistribute / (basicAttackStates.Count - 1);
        foreach (TimelordBossState state in basicAttackStates)
        {
            if (state != chosenState)
            {
                basicAttackProbabilities[state] += probabilityToAdd;
            }
        }

        NormalizeProbabilities();
        return chosenState;
    }

    private void NormalizeProbabilities()
    {
        foreach (TimelordBossState state in basicAttackStates)
        {
            basicAttackProbabilities[state] = Mathf.Clamp(basicAttackProbabilities[state], 0f, 1f);
        }

        float totalProbability = basicAttackProbabilities.Values.Sum();
        if (totalProbability == 0)
        {
            InitializeBasicAttackProbabilities();
        }
        else
        {
            foreach (TimelordBossState state in basicAttackStates)
            {
                basicAttackProbabilities[state] /= totalProbability;
            }
        }
    }

    private void InitializeBasicAttackProbabilities()
    {
        basicAttackProbabilities = new Dictionary<TimelordBossState, float>();
        foreach (TimelordBossState state in basicAttackStates)
        {
            basicAttackProbabilities.Add(state, 1f / basicAttackStates.Count);
        }
    }

    public TimelordBossState GetRandomSpecialAttackState()
    {
        if (availableSpecialAttacks.Count == 0)
        {
            if (isPhase2)
            {
                InitializeSpecialAttacksPhase2();
                availableSpecialAttacks = availableSpecialAttacksPhase2;
            }
            else
            {
                InitializeSpecialAttacksPhase1();
                availableSpecialAttacks = availableSpecialAttacksPhase1;
            }
        }

        int randomIndex = Random.Range(0, availableSpecialAttacks.Count);
        TimelordBossState chosenSpecialAttack = availableSpecialAttacks[randomIndex];
        availableSpecialAttacks.RemoveAt(randomIndex);
        return chosenSpecialAttack;
    }

    public void IncrementBasicAttackCounter()
    {
        basicAttackCounter++;
        if (basicAttackCounter >= basicAttacksPerCycle)
        {
            basicAttackCounter = 0;
        }
    }



    public void StartPhase2()
    {
        isPhase2 = true;
        shouldTransitionToPhase2 = false;
        SpawnUpgradeOrb();
        basicAttackStates = basicAttackStatesPhase2;
        availableSpecialAttacks = availableSpecialAttacksPhase2;
        shouldTransitionToPhase2 = false;

        basicAttackCounter = 0;

        StopAllConstantAbilities();
        ClearConstantAbilities();
        InitializeConstantAbilitiesPhase2();
    }

    private void InitializeConstantAbilitiesPhase2()
    {
        InitializeConstantAbility("spawnClockMob", 5f);
        InitializeConstantAbility("spawnBlackhole", 2f);
        InitializeConstantAbility("spawnTimeRift", 6f);
    }

    private void ClearConstantAbilities()
    {
        constantAbilityCoroutines.Clear();
        constantAbilityEnabled.Clear();
    }

    private void StopAllConstantAbilities()
    {
        List<string> abilityNames = new List<string>(constantAbilityCoroutines.Keys);

        foreach (string abilityName in abilityNames)
        {
            if (constantAbilityCoroutines.ContainsKey(abilityName))
            {
                StopCoroutine(constantAbilityCoroutines[abilityName]);
                SetConstantAbilityStatus(abilityName, false);
                Debug.Log("Stopped coroutine for: " + abilityName);
            }
        }
    }

    public TimelordTakeABreakState CreateTakeABreakState(float waitTime)
    {
        return new TimelordTakeABreakState(stateMachine, this, waitTime);
    }
}
