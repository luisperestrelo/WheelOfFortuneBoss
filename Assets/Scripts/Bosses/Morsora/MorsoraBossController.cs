using UnityEngine;

public class MorsoraBossController : MonoBehaviour
{
    [SerializeField] private SpawnTentacleSnapAbility spawnTentacleSnapAbility;

    private void Start()
    {
        spawnTentacleSnapAbility = GetComponent<SpawnTentacleSnapAbility>();
    }
} 