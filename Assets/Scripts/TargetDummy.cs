using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummy : MonoBehaviour
{
    [SerializeField] private GameObject upgradeOrb;
    [SerializeField] private Transform player;
    [SerializeField] private CircularPath playerPath;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerPath = FindObjectOfType<CircularPath>();
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
}
