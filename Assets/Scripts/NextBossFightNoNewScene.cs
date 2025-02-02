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

    [SerializeField] private GameObject UICanvas;
    [SerializeField] private GameObject endGameMenu;
    [SerializeField] private ParticleSystem spawnParticle;

    [SerializeField] private AudioClip spawnSfx;
    [SerializeField] private AudioClip spawnSfx2;

    private bool isEndlessMode = false;


    [SerializeField] private float delayBetweenBosses = 3f;

    private GameObject currentBoss;
    private int currentBossIndex = 0;

    private BossHealth bossHealth;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        UICanvas = GameObject.Find("UICanvas");
        endGameMenu = UICanvas.transform.Find("EndGameMenu").gameObject;
    }

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
        StartCoroutine(SpawnVfxAfterDelay(delayBetweenBosses - 4.5f));
        if (currentBossIndex == 2 && !isEndlessMode)
        {
            isEndlessMode = true;
            Time.timeScale = 0f; // Pause the game
            endGameMenu.SetActive(true);
        }
    }

    IEnumerator SpawnNextBossAfterDelay(float delay)
    {
        timerText.gameObject.SetActive(true);
        MusicPlayer.instance.LoadProfile(profiles[(currentBossIndex + 1) % bossPrefabs.Count]);
        float timeElapsed = 0;
        while (timeElapsed < delay)
        {
            timeElapsed += Time.deltaTime;
            timerText.text = Mathf.CeilToInt(delay - timeElapsed).ToString();
            yield return null;
        }
        timerText.gameObject.SetActive(false);
        //currentBossIndex++;
        currentBossIndex = (currentBossIndex + 1) % bossPrefabs.Count; // loop around

         if (AbilityObjectManager.Instance != null)
        {
            AbilityObjectManager.Instance.DestroyAllFatTentacles(); // TODO: Despawn animation
            AbilityObjectManager.Instance.DestroyAllFlails(); // TODO: Despawn animation
            AbilityObjectManager.Instance.DisableAllPortals();
            AbilityObjectManager.Instance.DestroyAllBigWavesOfDoom();
            AbilityObjectManager.Instance.DisableAllRotatingOrbs();
            AbilityObjectManager.Instance.DestroyAllTelegraphs();
        } 

        SpawnNextBoss();
        HealPlayerToFull();
    }

    private IEnumerator SpawnVfxAfterDelay(float delay)
    {
        float timeElapsed = 0;
        while(timeElapsed < delay)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Instantiate(spawnParticle, bossSpawnPoint.position, Quaternion.identity);
        SFXPool.instance.PlaySound(spawnSfx, SFXPool.MixGroup.ui);
        while(timeElapsed < delay+3)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        SFXPool.instance.PlaySound(spawnSfx2, SFXPool.MixGroup.ui);
    }

    void SpawnNextBoss()
    {
        if (currentBossIndex < bossPrefabs.Count)
        {
            //Timelord spawns at a different position... theres some weird stuff going on 
            if (bossPrefabs[currentBossIndex].name == "Timelord-Skeleton-Merged")
            {
                currentBoss = Instantiate(bossPrefabs[currentBossIndex], new Vector3(0.12f, -4.93f, 0), Quaternion.Euler(-45f, 0, 1.12f));
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
