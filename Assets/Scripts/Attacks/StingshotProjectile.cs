using UnityEngine;

public class StingshotProjectile : BaseProjectile
{
    private float poisonDamage;
    private float poisonDuration;



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out var health) && health.gameObject.tag != "Player")
        {
            EnemyBuffManager manager = health.GetComponent<EnemyBuffManager>();
            if (manager != null)
            {
                manager.ApplyBuff(new PoisonBuff(poisonDuration, poisonDamage));
            }

            
        }
        

        base.OnTriggerEnter2D(collision);

        //apply poison
    }

    public void SetPoisonStats(float damage, float duration)
    {
        poisonDamage = damage;
        poisonDuration = duration;
    }
}

