using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTestScript : MonoBehaviour
{
    [SerializeField] private WheelManager wheelManager;
    [SerializeField] private Field fieldToAdd;
    [SerializeField] private Field fieldToRemove;
    [SerializeField] private GameObject upgradeOrbPrefab;
    [SerializeField] private Player player;
    [SerializeField] private CircularPath playerPath;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerPath = FindObjectOfType<CircularPath>();
        wheelManager = FindObjectOfType<WheelManager>();
    }

    public void AddFieldToWheel()
    {
        wheelManager.AddField(fieldToAdd);
    }

    public void AddFieldToWheelAtIndex()
    {
        wheelManager.AddField(fieldToAdd, 3);
    }

    public void RemoveFieldFromWheel()
    {
        //wheelManager.RemoveField(fieldToRemove);
        wheelManager.RemoveField(0);
    }

    public void RemoveFieldFromWheelTemporarily()
    {
        wheelManager.ReplaceFieldTemporarily(0, fieldToAdd, 3f);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            AddFieldToWheel();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RemoveFieldFromWheel();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddFieldToWheelAtIndex();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveFieldFromWheelTemporarily();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            RunManager.Instance.EndFight();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnUpgradeOrb();
        }
    }

    private void SpawnUpgradeOrb()
    {
        Vector3 toPlayer = player.transform.position - playerPath.GetCenter();
        float currentAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;
        currentAngle += 180f;

        float x = playerPath.GetCenter().x + playerPath.GetRadius() * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = playerPath.GetCenter().y + playerPath.GetRadius() * Mathf.Sin(currentAngle * Mathf.Deg2Rad);
        Vector3 spawnPosition = new Vector3(x, y, transform.position.z);

        Instantiate(upgradeOrbPrefab, spawnPosition, Quaternion.identity);
    }
}
