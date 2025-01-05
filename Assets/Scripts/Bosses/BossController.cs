using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [SerializeField] private FireSlashAbility fireSlashAbility;
    [SerializeField] private float timeBetweenSlashes = 2f;
    [SerializeField] private GameObject[] fields;

    private BossStateMachine stateMachine;

    private Coroutine fireSlashCoroutine;

    private void Start()
    {
        stateMachine = new BossStateMachine();

        // Define states
        IdleState idleState = new IdleState(stateMachine, this);
        FireFieldsState fireFieldsState = new FireFieldsState(stateMachine, this);
        ShockwaveState shockwaveState = new ShockwaveState(stateMachine, this);
        GhostSummonState ghostSummonState = new GhostSummonState(stateMachine, this);
        SpearAndShockwaveState spearAndShockwaveState = new SpearAndShockwaveState(stateMachine, this);
        LaserSweepState laserSweepState = new LaserSweepState(stateMachine, this);
        MinionSummonState minionSummonState = new MinionSummonState(stateMachine, this);

        // Start in the Idle state (which will immediately transition to the first real state)
        stateMachine.Initialize(idleState);
        StartFireSlashCoroutine();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
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
    public int ChangeRandomFieldToFire()
    {
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
        foreach (GameObject field in fields)
        {
            field.GetComponent<WheelArea>().StopOnFire();
        }
    }

    public GameObject[] GetFields()
    {
        return fields;
    }
}