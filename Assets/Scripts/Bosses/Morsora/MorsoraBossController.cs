using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MorsoraBossController : MonoBehaviour
{

    [SerializeField] private float bossHealthUpgradeInterval = 0.1f;


    public Animator anim { get; private set; }
    public MorsoraBossStateMachine stateMachine { get; private set; }


    public SpawnTentacleSnapAbility spawnTentacleSnapAbility;
    public SpawnFatTentacleAbility spawnFatTentacleAbility;
    public SweepFatTentaclesAbility sweepFatTentaclesAbility;
    public SpawnTentacleSpitterAbility spawnTentacleSpitterAbility;
    public SpawnTentacleShield spawnTentacleShield;
    public SpawnRangedMinionsAbility spawnRangedMinions;
    public ThrowChakramAbility throwChakramAbility;

    public MorsoraIdleState idleState { get; private set; }
    public MorsoraLightScytheSwingState lightScytheSwingState { get; private set; }
    public MorsoraDarkScytheSwingState darkScytheSwingState { get; private set; }
    public MorsoraScytheComboState scytheComboState { get; private set; }
    public MorsoraShockwavesState shockwavesState { get; private set; }
    public MorsoraShockwaveWithTentaclesState shockwaveWithTentaclesState { get; private set; }
    public MorsoraTentacleHellState tentacleHellState { get; private set; }
    public MorsoraTentacleHellPhaseTwoState tentacleHellPhaseTwoState { get; private set; }
    public MorsoraRadialTentacleSlamState radialTentacleSlamState { get; private set; }
    public MorsoraTentacleSpiralState tentacleSpiralState { get; private set; }
    public MorsoraBossState placeholderState { get; private set; }
    public MorsoraThrowChakramState throwChakramState { get; private set; }
    public MorsoraTentacleShieldState tentacleShieldState { get; private set; }

    private Dictionary<string, Coroutine> constantAbilityCoroutines = new Dictionary<string, Coroutine>();
    private Dictionary<string, bool> constantAbilityEnabled = new Dictionary<string, bool>();

    public int basicAttackCounter = 0;
    private int basicAttacksPerCycle = 4; // 1 more than how many basic attacks there are, cos we increment first   
    private List<MorsoraBossState> availableSpecialAttacksPhase1;
    private List<MorsoraBossState> availableSpecialAttacksPhase2;
    private List<MorsoraBossState> availableSpecialAttacks;

    private float chanceAdjustment = 0.2f;

    public List<MorsoraBossState> basicAttackStatesPhase1;
    public List<MorsoraBossState> basicAttackStatesPhase2;
    public List<MorsoraBossState> basicAttackStates;
    public Dictionary<MorsoraBossState, float> basicAttackProbabilities;

    private BossHealth health;
    [Range(0, 1)]
    [SerializeField] private List<float> upgradeOrbSpawnHealthThresholds = new List<float>();
    [SerializeField] private float bossPhase2HealthThreshold = 0.5f;
    private bool isPhase2 = false;
    public bool shouldTransitionToPhase2 { get; private set; } = false;

    [SerializeField] private GameObject upgradeOrb;
    [SerializeField] public Transform player { get; private set; }
    [SerializeField] private CircularPath playerPath;

    public WheelManager wheelManager { get; private set; }

    [SerializeField] private TentacleShieldBusterField shieldBusterField;

    private void Awake()
    {
        wheelManager = FindObjectOfType<WheelManager>();

        spawnTentacleShield = GetComponent<SpawnTentacleShield>();
        spawnRangedMinions = GetComponent<SpawnRangedMinionsAbility>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerPath = FindObjectOfType<CircularPath>();

        stateMachine = new MorsoraBossStateMachine();
        idleState = new MorsoraIdleState(stateMachine, this, "Idle");
        lightScytheSwingState = new MorsoraLightScytheSwingState(stateMachine, this, "ScytheSwingLight");
        darkScytheSwingState = new MorsoraDarkScytheSwingState(stateMachine, this, "ScytheSwingDark");
        scytheComboState = new MorsoraScytheComboState(stateMachine, this, "Idle");
        shockwavesState = new MorsoraShockwavesState(stateMachine, this, "StaffGround");
        shockwaveWithTentaclesState = new MorsoraShockwaveWithTentaclesState(stateMachine, this, "StaffGroundWithTentacles");
        tentacleHellState = new MorsoraTentacleHellState(stateMachine, this, "Idle");
        radialTentacleSlamState = new MorsoraRadialTentacleSlamState(stateMachine, this, "Idle");
        tentacleSpiralState = new MorsoraTentacleSpiralState(stateMachine, this, "Idle");
        placeholderState = new MorsoraBossState(stateMachine, this, "Idle");
        throwChakramState = new MorsoraThrowChakramState(stateMachine, this, "ThrowChakram");
        tentacleShieldState = new MorsoraTentacleShieldState(stateMachine, this, "Idle", shieldBusterField);
        tentacleHellPhaseTwoState = new MorsoraTentacleHellPhaseTwoState(stateMachine, this, "Idle");

        // Initialize basic attack states
        basicAttackStatesPhase1 = new List<MorsoraBossState>
        {
            lightScytheSwingState,
            darkScytheSwingState//,
            //throwChakramState
        };

        basicAttackStatesPhase2 = new List<MorsoraBossState>
        {
            lightScytheSwingState,
            darkScytheSwingState

        };

        basicAttackStates = basicAttackStatesPhase1;

        health = GetComponent<BossHealth>();
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spawnTentacleSnapAbility = GetComponent<SpawnTentacleSnapAbility>();
        spawnFatTentacleAbility = GetComponent<SpawnFatTentacleAbility>();
        sweepFatTentaclesAbility = GetComponent<SweepFatTentaclesAbility>();
        spawnTentacleSpitterAbility = GetComponent<SpawnTentacleSpitterAbility>();

        throwChakramAbility = GetComponent<ThrowChakramAbility>();


        InitializeBasicAttackProbabilities();
        InitializeConstantAbility("SpawnTentacleSnap", 3.5f);
        InitializeConstantAbility("SpawnTentacleSpitter", 8f);

        stateMachine.Initialize(idleState);
        //stateMachine.ChangeState(tentacleShieldState);
        //stateMachine.Initialize(tentacleShieldState);

        // Initialize and start constant abilities

        //spawnTentacleSnapAbility.SpawnTentacleSpiral(0f, 0.2f, SpawnDirection.Clockwise);
        //spawnTentacleSnapAbility.SpawnCircleOfTentaclesWithGap(0f, 90f, 15f, 1);
        //stateMachine.ChangeState(scytheComboState);

        InitializeSpecialAttacksPhase1();
        InitializeSpecialAttacksPhase2();
        availableSpecialAttacks = availableSpecialAttacksPhase1;

    }
    private void Update()
    {
        stateMachine.currentState.Update();
        CheckIfUpgradeThreshold();
        CheckPhase2Transition();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L)) // for testing
        {
            //stateMachine.ChangeState(radialTentacleSlamState);
            //stateMachine.ChangeState(tentacleHellPhaseTwoState);
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
        availableSpecialAttacksPhase1 = new List<MorsoraBossState>
        {
            tentacleHellState,
            radialTentacleSlamState,
            shockwavesState,
            tentacleSpiralState,
            //scytheComboState // I would like this to be a sequence of Scythe Slashes but much faster
                              // But need a way to indiciate that
        };
    }

    private void InitializeSpecialAttacksPhase2()
    {
        availableSpecialAttacksPhase2 = new List<MorsoraBossState>
        {
            radialTentacleSlamState, // maybe remove this one from P2
            shockwaveWithTentaclesState,
            tentacleHellPhaseTwoState,
            //tentacleSpiralState,
            //scytheComboState // I would like this to be a sequence of Scythe Slashes but much faster
                              // But need a way to indiciate that
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
                    case "SpawnTentacleSnap":
                        spawnTentacleSnapAbility.SpawnTentaclesRandomlyAroundPlayerWithDelay(3, 1, 30, 0.25f);
                        break;
                    case "SpawnTentacleSpitter":
                        spawnTentacleSpitterAbility.SpawnTentacleSpitter(Random.Range(160, 380), 3); // dont spawn behind boss
                        break;
                    case "Phase2TentacleSnap":
                        spawnTentacleSnapAbility.SpawnTentaclesRandomlyAroundPlayerWithDelay(7, 1, 30, 0.25f);
                        break;
                    case "Phase2TentacleSpitter":
                        float randomAngle = Random.Range(160, 360);
                        spawnTentacleSpitterAbility.SpawnTentacleSpitter(randomAngle, 3); // dont spawn behind boss

                        // Calculate the mirrored angle
                        float mirroredAngle = 360 - (randomAngle - 180);

                        // Ensure a minimum distance of 15 degrees between the two angles
                        if (Mathf.Abs(mirroredAngle - randomAngle) < 15f)
                        {
                            if (mirroredAngle > randomAngle)
                            {
                                mirroredAngle = randomAngle + 15f;
                            }
                            else
                            {
                                mirroredAngle = randomAngle - 15f;
                            }
                        }

                        // Clamp the mirrored angle to stay within the allowed range
                        mirroredAngle = Mathf.Clamp(mirroredAngle, 160, 360);

                        spawnTentacleSpitterAbility.SpawnTentacleSpitter(mirroredAngle, 3);
                        break;
                    case "Phase2Chakram":
                        throwChakramAbility.ThrowChakramAtPredictedPlayerPosition(0f);
                        throwChakramAbility.ThrowChakramAtPredictedPlayerPosition(3f);
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

    public MorsoraBossState GetRandomState()
    {
        List<MorsoraBossState> availableStates = new List<MorsoraBossState>
        {
            lightScytheSwingState,
            darkScytheSwingState,
            shockwavesState,
            shockwaveWithTentaclesState
        };

        int randomIndex = Random.Range(0, availableStates.Count);

        return availableStates[randomIndex];
    }

    public void DisableAllConstantAbilities()
    {
        // Create a copy of the keys
        List<string> abilityNames = new List<string>(constantAbilityEnabled.Keys);

        foreach (string abilityName in abilityNames)
        {
            SetConstantAbilityStatus(abilityName, false);
        }
    }

    public void RestartAllConstantAbilities()
    {
        // Create a copy of the keys
        List<string> abilityNames = new List<string>(constantAbilityEnabled.Keys);

        foreach (string abilityName in abilityNames)
        {
            SetConstantAbilityStatus(abilityName, true);
        }
    }

    public MorsoraBossState GetRandomBasicAttackState()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;
        MorsoraBossState chosenState = null;

        // Select a state based on cumulative probabilities
        foreach (KeyValuePair<MorsoraBossState, float> entry in basicAttackProbabilities)
        {
            cumulativeProbability += entry.Value;
            if (randomValue <= cumulativeProbability)
            {
                chosenState = entry.Key;
                break;
            }
        }

        // Adjust probabilities
        float probabilityToDistribute = basicAttackProbabilities[chosenState] * chanceAdjustment;
        basicAttackProbabilities[chosenState] -= probabilityToDistribute;

        float probabilityToAdd = probabilityToDistribute / (basicAttackStates.Count - 1);
        foreach (MorsoraBossState state in basicAttackStates)
        {
            if (state != chosenState)
            {
                basicAttackProbabilities[state] += probabilityToAdd;
            }
        }

        // Clamp and normalize probabilities
        NormalizeProbabilities();

        return chosenState;
    }

    private void NormalizeProbabilities()
    {
        // Clamp probabilities
        foreach (MorsoraBossState state in basicAttackStates)
        {
            basicAttackProbabilities[state] = Mathf.Clamp(basicAttackProbabilities[state], 0f, 1f);
        }

        // Normalize
        float totalProbability = basicAttackProbabilities.Values.Sum();
        if (totalProbability == 0)
        {
            // Handle edge case where all probabilities are 0
            InitializeBasicAttackProbabilities();
        }
        else
        {
            foreach (MorsoraBossState state in basicAttackStates)
            {
                basicAttackProbabilities[state] /= totalProbability;
            }
        }
    }

    private void InitializeBasicAttackProbabilities()
    {
        // Initialize probabilities
        basicAttackProbabilities = new Dictionary<MorsoraBossState, float>();
        foreach (MorsoraBossState state in basicAttackStates)
        {
            basicAttackProbabilities.Add(state, 1f / basicAttackStates.Count); // Equal initial probabilities
        }
    }

    public MorsoraBossState GetRandomSpecialAttackState()
    {
        if (availableSpecialAttacks.Count == 0)
        {
            if (isPhase2)
            {
                InitializeSpecialAttacksPhase2(); // Reset special attacks for phase 2
                availableSpecialAttacks = availableSpecialAttacksPhase2;
            }
            else
            {
                InitializeSpecialAttacksPhase1(); // Reset special attacks for phase 1
                availableSpecialAttacks = availableSpecialAttacksPhase1;
            }
        }

        int randomIndex = Random.Range(0, availableSpecialAttacks.Count);
        MorsoraBossState chosenSpecialAttack = availableSpecialAttacks[randomIndex];
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

    public void TentacleShieldChargeCompleted()
    {
        tentacleShieldState.IncrementChargeCount();
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

        // Stop and clear existing constant abilities
        StopAllConstantAbilities();
        ClearConstantAbilities();

        // Initialize and start new constant abilities for Phase 2
        InitializeConstantAbilitiesPhase2();
    }

    private void InitializeConstantAbilitiesPhase2()
    {
        InitializeConstantAbility("Phase2TentacleSnap", 5f);
        InitializeConstantAbility("Phase2TentacleSpitter", 2f);
        InitializeConstantAbility("Phase2Chakram", 6f);
    }

    private void ClearConstantAbilities()
    {
        constantAbilityCoroutines.Clear();
        constantAbilityEnabled.Clear();
    }

    private void StopAllConstantAbilities()
    {
        // Create a copy of the keys to avoid issues when modifying the dict 
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
}