using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelArea : MonoBehaviour
{

    [SerializeField] protected bool _isPlayerInArea = false;
    [SerializeField] protected Transform _player;
    [SerializeField] protected PlayerCombat _playerCombat;
    [SerializeField] protected ParticleSystem _onFireParticles;
    [SerializeField] protected Health _playerHealth;

    protected bool _isOnFire = false;

    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerCombat = _player.GetComponent<PlayerCombat>();
        _onFireParticles = GetComponentInChildren<ParticleSystem>();
        _playerHealth = _player.GetComponent<Health>();
    }

    protected virtual void Update()
    {

        if (_isPlayerInArea && _isOnFire)
        {
            _playerHealth.TakeDamage(20f * Time.deltaTime);
        }

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInArea = true;
            if (_isOnFire)
            {

            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInArea = false;
        }
    }

    public void SetOnFire()
    {
        _onFireParticles.Play();
        _isOnFire = true;
    }

    public void StopOnFire()
    {
        _onFireParticles.Stop();
        _isOnFire = false;
    }
}
