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

    [SerializeField] private float delayBetweenBosses = 3f;

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
        StartCoroutine(SpawnNextBossAfterDelay(delayBetweenBosses));
    }

    IEnumerator SpawnNextBossAfterDelay(float delay)
    {
        timerText.gameObject.SetActive(true);
        MusicPlayer.instance.LoadProfile(profiles[currentBossIndex + 1]);
        float timeElapsed = 0;
        while (timeElapsed < delay)
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
            //Timelord spawns at a different position... theres some weird stuff going on 
            if (currentBossIndex == 1)
            {
                currentBoss = Instantiate(bossPrefabs[currentBossIndex], new Vector3(0.12f, -4.93f, 0), Quaternion.Euler(-45f,0,1.12f));
            }
            else
            {
                currentBoss = Instantiate(bossPrefabs[currentBossIndex], bossSpawnPoint.position, bossSpawnPoint.rotation);
            }

            bossHealth = currentBoss.GetComponent<BossHealth>();
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
