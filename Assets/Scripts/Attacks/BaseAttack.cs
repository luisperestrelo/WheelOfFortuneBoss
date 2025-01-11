using UnityEngine;

public abstract class BaseAttack : ScriptableObject
{
    public float BaseDamage = 1;
    public float FireRate = 1;
    [SerializeField] private AudioClip shootSfx;
    //public abstract void PerformAttack(PlayerCombat playerCombat);

    //TODO: @Josh this is where I put the sounds for attacks
    public virtual void PerformAttack(PlayerCombat playerCombat)
    {
        if (shootSfx != null)
        {
            playerCombat.shootAudioSource.PlayOneShot(shootSfx);
            playerCombat.shootAudioSource.pitch = Random.Range(0.9f, 1.3f);
        }
    }
}