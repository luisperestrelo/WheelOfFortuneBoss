using UnityEngine;

public class Spear : EnemyProjectile
{

    // Constructor to initialize values
    public void Initialize(float speed, float damage, float lifeTime)
    {
        this.speed = speed;
        this.damage = damage;
        this.lifeTime = lifeTime;
        Destroy(gameObject, lifeTime);
    }

    protected override void Update()
    {
        base.Update();
        transform.Translate(Vector3.up * (speed * Time.deltaTime)); 
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
