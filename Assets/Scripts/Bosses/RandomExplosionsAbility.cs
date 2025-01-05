using UnityEngine;

public class RandomExplosionsAbility : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosionTelegraphPrefab;
    [SerializeField] private int numberOfExplosions = 5;
    [SerializeField] private float telegraphDuration = 1f;
    [SerializeField] private float minDistanceFromBoss = 2f;
    [SerializeField] private float maxDistanceFromBoss = 5f;
    [SerializeField] private float explosionRadius = 1.5f; // not being used as the explosion and telegraph are already prepared to fit 

    [SerializeField] private float explosionDamage = 20f;
    [SerializeField] private float explosionLifeTime = 0.3f;

    public void TriggerExplosions()
    {
        for (int i = 0; i < numberOfExplosions; i++)
        {
            Vector2 randomPosition = GetRandomPositionAroundBoss();

            // Instantiate the telegraph
            GameObject telegraph = Instantiate(explosionTelegraphPrefab, randomPosition, Quaternion.identity);
            telegraph.GetComponent<ExplosionTelegraph>().SetTelegraphDuration(telegraphDuration);
            //telegraph.transform.localScale = Vector3.one * explosionRadius * 2f; // Scale telegraph to explosion size
            //theyre already prepared to fit

            // Start a coroutine to spawn the explosion after the telegraph duration
            StartCoroutine(SpawnExplosionAfterDelay(randomPosition, telegraphDuration));
        }
    }

    private Vector2 GetRandomPositionAroundBoss()
    {
        Vector2 randomPoint;
        do
        {
            randomPoint = (Vector2)transform.position + Random.insideUnitCircle.normalized * Random.Range(minDistanceFromBoss, maxDistanceFromBoss);
        } while (Vector2.Distance(randomPoint, transform.position) < minDistanceFromBoss);

        return randomPoint;
    }

    private System.Collections.IEnumerator SpawnExplosionAfterDelay(Vector2 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Instantiate the explosion
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        //explosion.transform.localScale = Vector3.one * explosionRadius * 2f; // Scale explosion
        //theyre already prepared to fit    

        // Set parameters on the explosion
        Explosion explosionComponent = explosion.GetComponent<Explosion>();
        explosionComponent.SetDamage(explosionDamage);
        explosionComponent.SetLifeTime(explosionLifeTime);
    }

    //draw gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromBoss);
        Gizmos.DrawWireSphere(transform.position, maxDistanceFromBoss);
    }
} 