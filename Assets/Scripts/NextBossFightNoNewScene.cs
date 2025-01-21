using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NextBossFightNoNewScene : MonoBehaviour
{
    public List<GameObject> bossPrefabs;
    public Transform bossSpawnPoint;

    [SerializeField] private List<SoundProfile> profiles;
    [SerializeField] private TextMeshProUGUI timerText;

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
        StartCoroutine(SpawnNextBossAfterDelay(5f));
    }

    IEnumerator SpawnNextBossAfterDelay(float delay)
    {
        timerText.gameObject.SetActive(true);
        float timeElapsed = 0;
        while(timeElapsed < delay)
        {
            timeElapsed += Time.deltaTime;
            timerText.text = Mathf.CeilToInt(delay - timeElapsed).ToString();
            yield return null;
        }
        timerText.gameObject.SetActive(false);
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
            MusicPlayer.instance.LoadProfile(profiles[currentBossIndex]);
            MusicPlayer.instance.StartSection(MusicPlayer.MusicSection.fight);
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
