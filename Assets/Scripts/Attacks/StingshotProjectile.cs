using UnityEngine;

public class StingshotProjectile : BaseProjectile
{
/*     private float poisonDamage;
    private float poisonDuration; */



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
/*         if (collision.gameObject.TryGetComponent<Health>(out var health) && health.gameObject.tag != "Player")
        {
            BuffManager manager = health.GetComponent<BuffManager>();
            if (manager != null)
            {
                manager.ApplyBuff(new PoisonBuff(poisonDamage, poisonDuration));
            }

            
        } */
        

        base.OnTriggerEnter2D(collision);

        //apply poison
    }

/*     public void SetPoisonStats(float damage, float duration)
    {
        poisonDamage = damage;
        poisonDuration = duration;
    } */
}

