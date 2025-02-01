using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeRift : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Damage per second inflicted if the player remains in the rift.")]
    [SerializeField] private float damagePerSecond = 10f;

    [Tooltip("Interval in seconds between consecutive damage ticks while standing inside the rift.")]
    [SerializeField] private float damageInterval = 0.4f;

    // Cooldown so that if the player steps in/out quickly, they don't get "instant" damage multiple times.
    // Example: 1 second cooldown on immediate damage
    [SerializeField] private float instantDamageCooldown = 0.4f;

    private bool _isGrown = false;
    private bool _isPlayerInRift = false;
    private float _damageTimer = 0f;

    private Collider2D _collider2D;
    private PlayerHealth _playerHealth;

    // Track the last time we applied "instant damage" so it doesn't spam
    private float _lastInstantDamageTime = -999f;

    private void Awake()
    {
        // Grab the collider and disable it until animation says we've fully grown.
        _collider2D = GetComponent<Collider2D>();
        if (_collider2D != null)
        {
            _collider2D.enabled = false;
        }
    }

    private void Start()
    {

        _playerHealth = FindObjectOfType<PlayerHealth>(); // fallback
    }

    private void Update()
    {
        // Only do damage if we've grown.
        if (!_isGrown) return;

        if (_isPlayerInRift)
        {
            _damageTimer += Time.deltaTime;
            if (_damageTimer >= damageInterval)
            {
                if (_playerHealth != null)
                {
                    // Damage over time
                    float damageAmount = damagePerSecond * damageInterval;
                    _playerHealth.TakeDamage(damageAmount, false);
                }
                _damageTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Called from the animation event in TimeRiftAnimationEvents
    /// to enable collision (& thereby damage) once the TimeRift is "fully grown."
    /// </summary>
    public void EnableDamage()
    {
        _isGrown = true;
        if (_collider2D != null)
        {
            _collider2D.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isGrown) return; 

        if (other.CompareTag("Player"))
        {
            _playerHealth = other.GetComponent<PlayerHealth>();
            if (_playerHealth != null)
            {
                // Apply an immediate damage chunk IF enough time passed since last immediate hit
                if (Time.time - _lastInstantDamageTime >= instantDamageCooldown)
                {
                    float immediateDamage = damagePerSecond * damageInterval;
                    _playerHealth.TakeDamage(immediateDamage, false);
                    _lastInstantDamageTime = Time.time;
                }
            }

            _isPlayerInRift = true;
            _damageTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_isGrown) return; 

        if (other.CompareTag("Player"))
        {
            _isPlayerInRift = false;
            _damageTimer = 0f;
        }
    }
}
