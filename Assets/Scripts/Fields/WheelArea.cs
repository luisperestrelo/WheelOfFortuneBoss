using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelArea : MonoBehaviour
{
    protected bool _isOnFire = false;
    [SerializeField] private ParticleSystem _onFireParticles;   

    protected virtual void Start()
    {
        _onFireParticles = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Called each frame the player is in this area.
    /// </summary>
    public virtual void OnUpdate(PlayerCombat player)
    {
        if (_isOnFire) player.health.TakeDamage(20 * Time.deltaTime);
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
