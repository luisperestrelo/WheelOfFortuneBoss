using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWaveOfDoom : MonoBehaviour
{
    [Header("Movement Speed")]
    public float speed = 3f;

    [Header("Lifetime")]
    public float lifetime = 50f;

    private float _elapsedTime = 0f;

    
    [Header("Damage Per Second")] 
    [Tooltip("This should be tuned in a way so that the player tanks the whole wave, they survive but it's a lot of damage. This ability should be a big deal")]
    public int damagePerSecond = 10;

    [Header("Damage Interval (seconds)")]
    [Tooltip("How often damage is inflicted while the player is in the wave.")]
    public float damageInterval = 0.4f;
    private float _damageTimer = 0f;
    private bool _isPlayerInWave = false;

    private PlayerHealth _playerHealth;

    void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }


    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (_isPlayerInWave)
        {
            _damageTimer += Time.deltaTime;

            if (_damageTimer >= damageInterval)
            {
                if (_playerHealth != null)
                {
                    _playerHealth.TakeDamage(damagePerSecond * damageInterval);
                }
                _damageTimer = 0f;
            }


        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damagePerSecond * damageInterval);

            _isPlayerInWave = true;
            _damageTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInWave = false;
            _damageTimer = 0f;
        }
    }
}
