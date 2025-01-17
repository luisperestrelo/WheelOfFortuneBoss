using UnityEngine;

public class MorsoraBossController : MonoBehaviour
{
    [SerializeField] private SpawnTentacleSnapAbility spawnTentacleSnapAbility;
    [SerializeField] private SweepFatTentaclesAbility sweepFatTentaclesAbility;

    private void Start()
    {
        spawnTentacleSnapAbility = GetComponent<SpawnTentacleSnapAbility>();
        sweepFatTentaclesAbility = GetComponent<SweepFatTentaclesAbility>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            sweepFatTentaclesAbility.StartSweep();
        }
    }
} 