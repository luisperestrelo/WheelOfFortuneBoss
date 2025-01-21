using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBossFightNoNewScene : MonoBehaviour
{
    public List<GameObject> bossPrefabs;
    public Transform bossSpawnPoint;

    private GameObject currentBoss;
    private int currentBossIndex = 0;

    private BossHealth bossHealth;
    private PlayerHealth playerHealth;

    private void Start()
    {
        bossSpawnPoint = FindObjectOfType<CircularPath>().transform;
        bossHealth = FindObjectOfType<BossHealth>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (bossHealth != null)
        {
            bossHealth.OnDie.AddListener(OnBossDefeated);
        }
    }


    public void OnBossDefeated()
    {
        StartCoroutine(SpawnNextBossAfterDelay(2f));
    }

    IEnumerator SpawnNextBossAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        currentBossIndex++;
        SpawnNextBoss();
        HealPlayerToFull(); 
    }

    void SpawnNextBoss()
    {
        if (currentBossIndex < bossPrefabs.Count)
        {
            currentBoss = Instantiate(bossPrefabs[currentBossIndex], bossSpawnPoint.position, bossSpawnPoint.rotation);
            bossHealth = currentBoss.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.OnDie.AddListener(OnBossDefeated);
            }
        }
        else
        {
            Debug.Log("All bosses defeated!");
        }
    }

    void HealPlayerToFull()
    {
        playerHealth.SetHealthToFull();
    }

    void Update()
    {

    }
}
