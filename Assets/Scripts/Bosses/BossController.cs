using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [SerializeField] private FireSlashAbility fireSlashAbility;
    [SerializeField] private float timeBetweenSlashes = 2f;
    [SerializeField] private GameObject[] fields;
    [SerializeField] private Health health;
    [SerializeField] private GameObject upgradeOrb;
    [SerializeField] private Transform player;
    [SerializeField] private CircularPath playerPath;

    [Header("Upgrade Orb Stuff")]
    [SerializeField] private float bossHealthUpgradeInterval = 0.25f;
    private int timesIncapacitated = 0;

    private BossStateMachine stateMachine;

    private Coroutine fireSlashCoroutine;

    private bool hasTriggeredIncapacitatedStateThisCycle = false;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
        playerPath = FindObjectOfType<CircularPath>();
        stateMachine = new BossStateMachine(this);
        stateMachine.Initialize(stateMachine.idleState);
        StartFireSlashCoroutine();
    }

    private void Update()
    {
        stateMachine.Update();
        //CheckIfUpgradeThreshold(); TODO: Mid-fight upgrades
    }

    private void CheckIfUpgradeThreshold()
    {
        float currentThreshold = 1.0f - (bossHealthUpgradeInterval * (timesIncapacitated + 1));
        
        if (health.GetCurrentHealth() <= health.GetMaxHealth() * currentThreshold &&
            stateMachine.currentState != stateMachine.incapacitatedState &&
            !hasTriggeredIncapacitatedStateThisCycle)
        {

            BossState nextState = stateMachine.currentState;
            Debug.Log(nextState + " is the next state");
            stateMachine.TriggerIncapacitatedState(nextState, 5f);
            timesIncapacitated++;

            SpawnUpgradeOrb();

            // hasTriggeredIncapacitatedStateThisCycle = true; // Shouldn't be necessary since we're changing the health threshold now
                                                               // but should keep an eye out
        }
    }

    private void SpawnUpgradeOrb()
    {
        Vector3 toPlayer = player.position - playerPath.GetCenter();
        float currentAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        currentAngle += 180f; 

        float x = playerPath.GetCenter().x + playerPath.GetRadius() * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = playerPath.GetCenter().y + playerPath.GetRadius() * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 spawnPosition = new Vector3(x, y, transform.position.z);

        Instantiate(upgradeOrb, spawnPosition, Quaternion.identity);
    }

    public void ChangeState(BossState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void StartFireSlashCoroutine()
    {
        fireSlashCoroutine = StartCoroutine(FireSlashCoroutine());
    }

    public void StopFireSlashCoroutine()
    {
        if (fireSlashCoroutine != null)
        {
            StopCoroutine(fireSlashCoroutine);
            fireSlashCoroutine = null;
        }
    }

    private IEnumerator FireSlashCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSlashes);
            fireSlashAbility.FireSlash();
        }
    }

    public void ActivateFireFields(int[] pattern)
    {
        if (fields.Length == 0)
        {
            Debug.LogError("No fields assigned to BossController!");
            return;
        }

        foreach (int fieldIndex in pattern)
        {
            if (fieldIndex >= 0 && fieldIndex < fields.Length)
            {
                fields[fieldIndex].GetComponent<WheelArea>().SetOnFire();
            }
            else
            {
                Debug.LogWarning("Invalid field index: " + fieldIndex);
            }
        }
    }

    // TODO: Not let it repeat field
    // TODO: Make it an actual ability like the others, but we can do that when we do the wheel refactor
    public int ChangeRandomFieldToFire()
    {
        return 0 ;
        if (fields.Length == 0)
            return 0;

        int randomIndex = Random.Range(0, fields.Length);
        WheelArea area = fields[randomIndex].GetComponent<WheelArea>();

        area.SetOnFire();

        Debug.Log("Field " + randomIndex + " is on fire");

        return randomIndex;
    }


    public void DeactivateFireFields()
    {
        return;
        foreach (GameObject field in fields)
        {
            field.GetComponent<WheelArea>().StopOnFire();
        }
    }

    public GameObject[] GetFields()
    {
        return fields;
    }

    public BossStateMachine GetStateMachine()
    {
        return stateMachine;
    }

    public void TriggerIncapacitatedState(BossState nextState, float duration)
    {

        /*       if (powerUpSpawner != null)
              {
                  powerUpSpawner.SpawnPowerUp(new Vector3(0, 0, 0)); 
              } */

        stateMachine.TriggerIncapacitatedState(nextState, duration);
    }

    public void ResetIncapacitatedStateTriggerFlag()
    {
        hasTriggeredIncapacitatedStateThisCycle = false;
    }
}