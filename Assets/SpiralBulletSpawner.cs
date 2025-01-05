using UnityEngine;
using System.Collections;

public class SpiralBulletSpawner2D : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float spawnRate = 0.05f;
    public float bulletSpeed = 5f;

    [Header("Spiral Settings")]
    public float spiralSpeed = 360f;
    public float startRadius = 0.5f;
    public float radiusGrowth = 0.05f;

    private float _angle = 0f;
    private float _currentRadius;
    private void Start()
    {
        _currentRadius = startRadius;
        StartCoroutine(SpawnSpiral());
    }

    private IEnumerator SpawnSpiral()
    {
        while (true)
        {
            float rad = _angle * Mathf.Deg2Rad;

            if (_angle % 360f < 270f || _angle % 360f > 360f) // safe radius is 270 to 360  
            {
                Vector2 spawnPos = new Vector2(
                    transform.position.x + _currentRadius * Mathf.Cos(rad),
                    transform.position.y + _currentRadius * Mathf.Sin(rad)
                );

                GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

                Vector2 direction = (spawnPos - (Vector2)transform.position).normalized;


                float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - 90f);

                Rigidbody2D rb2D = bullet.GetComponent<Rigidbody2D>();
                if (rb2D != null)
                {
                    rb2D.velocity = direction * bulletSpeed;
                }
            }


            _angle += spiralSpeed * Time.deltaTime;
            _currentRadius += radiusGrowth;

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
