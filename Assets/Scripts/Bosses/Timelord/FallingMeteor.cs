using UnityEngine;

public class FallingMeteor : MonoBehaviour
{
    [Header("Trajectory")]
    [Tooltip("The duration in seconds for the meteor to complete its fall.")]
    [SerializeField] private float fallDuration = 2.0f;

    [Tooltip("Controls how high the arc goes.")]
    [SerializeField] private float arcHeight = 2.0f;

    [Header("Telegraph (Scaling)")]
    [Tooltip("Scale at the start (meteor is still high up).")]
    [SerializeField] private float initialTelegraphScale = 0.25f;

    [Tooltip("Scale at the end (meteor hits the ground).")]
    [SerializeField] private float finalTelegraphScale = 1.2f;

    [Header("Damage Settings")]
    [SerializeField] private float explosionRadius = 1.5f;
    [SerializeField] private float meteorDamage = 10f;
    [SerializeField] private LayerMask damageLayerMask;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;

    // The telegraph is a separate object pinned on the ground at _targetPosition
    private Transform _telegraphTransform;

    private GameObject _impactVfxPrefab;

    // A 2D coordinate used for OverlapCircle to damage the player. Receives it from the spawner
    private Vector2 _damageCenter2D;

    private float _elapsedTime;

    // ------------------------------------------------------------------------
    //  Public Init
    // ------------------------------------------------------------------------
    /// <summary>
    /// Called from the spawner to set the specific ground location
    /// and the telegraph object that belongs to this meteor instance.
    /// </summary>
    public void Init(Vector3 targetPos, Transform telegraph, Vector2 damageCenter2D, GameObject impactVfxPrefab)
    {
        _targetPosition = targetPos;
        _telegraphTransform = telegraph;

        _startPosition = transform.position;

        _damageCenter2D = damageCenter2D;
        
        _impactVfxPrefab = impactVfxPrefab;

        if (_telegraphTransform != null)
        {
            _telegraphTransform.localScale = Vector3.one * initialTelegraphScale;
        }
    }

    private void Update()
    {
        if (_telegraphTransform == null)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;
        float t = _elapsedTime / fallDuration;

        if (t >= 1f)
        {
            // Snap to final position
            transform.position = _targetPosition;
            UpdateTelegraphScale(1f);

            Explode();
            return;
        }

        // Parabolic arc movement
        Vector3 nextPos = Vector3.Lerp(_startPosition, _targetPosition, t);
        float heightOffset = arcHeight * 4f * (t - t * t); // simple parabola shape
        nextPos.y += heightOffset;
        transform.position = nextPos;

        // It increases in size as the meteor falls.
        UpdateTelegraphScale(t);
    }

    // ------------------------------------------------------------------------
    //  Private Helpers
    // ------------------------------------------------------------------------
    private void UpdateTelegraphScale(float t)
    {
        float scale = Mathf.Lerp(initialTelegraphScale, finalTelegraphScale, t);
        _telegraphTransform.localScale = Vector3.one * scale;
    }

    private void Explode()
    {
        Debug.Log("Meteor impacted visually at " + transform.position);
        Instantiate(_impactVfxPrefab, _telegraphTransform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(_damageCenter2D, explosionRadius, damageLayerMask);
        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit " + hit.name);
            var health = hit.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(meteorDamage, false);
            }
        }

        if (_telegraphTransform != null)
        {
            Destroy(_telegraphTransform.gameObject);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_damageCenter2D, explosionRadius);
    }
}