using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpinMovement : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;
    [SerializeField] private float defaultRotationSpeed = 180f; // degrees per second
    [SerializeField] private float radius = 5f;

    [Header("Speed Boost Settings")]
    [SerializeField] private float boostAmount = 300f;
    [SerializeField] private float boostDuration = 1f;
    [SerializeField] private float boostCooldown = 3f;
    private float nextBoostTime = 0f;

    [Header("Speed Slow Settings")]
    [SerializeField] private float slowAmount = 60f;
    [SerializeField] private float slowDuration = 1f;
    [SerializeField] private float slowCooldown = 3f;
    private float nextSlowTime = 0f;

    [SerializeField] private bool drawLine = true;
    private float _currentAngle = 50f;
    private float _direction = 1f;
    private float _currentRotationSpeed;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _currentRotationSpeed = defaultRotationSpeed;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {

/*         if (Input.GetKeyDown(KeyCode.Space))
        {
            _direction *= -1f;
        } */

        if (Input.GetKeyDown(KeyCode.F))
        {
            _direction *= -1f;
        }

        if (Input.GetKeyDown(KeyCode.W) && Time.time > nextBoostTime)
        {
            StartCoroutine(SpeedBoostRoutine());
        }
        else if (Input.GetKeyDown(KeyCode.S) && Time.time > nextSlowTime)
        {
            StartCoroutine(SpeedSlowRoutine());
        }

        DrawLine();

        _currentAngle += _direction * _currentRotationSpeed * Time.deltaTime;
        _currentAngle %= 360f;

        if (anchorPoint == null)
        {
            return;
        }

        float x = anchorPoint.position.x + radius * Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
        float y = anchorPoint.position.y + radius * Mathf.Sin(_currentAngle * Mathf.Deg2Rad);

        transform.position = new Vector3(x, y, transform.position.z);
    }

    private IEnumerator SpeedBoostRoutine()
    {
        float oldSpeed = _currentRotationSpeed; // might be useful in a later version
        _currentRotationSpeed = boostAmount;

        nextBoostTime = Time.time + boostCooldown;

        yield return new WaitForSeconds(boostDuration);

        //_currentRotationSpeed = oldSpeed;
        _currentRotationSpeed = defaultRotationSpeed;
    }

    private IEnumerator SpeedSlowRoutine()
    {
        float oldSpeed = _currentRotationSpeed;
        _currentRotationSpeed = slowAmount;

        nextSlowTime = Time.time + slowCooldown;

        yield return new WaitForSeconds(slowDuration);

        //_currentRotationSpeed = oldSpeed;
        _currentRotationSpeed = defaultRotationSpeed;
    }

    // TODO: this is a very lazy implementation, but the idea would be to use this for attacks that are aimed at "in front" of the player
    public Vector3 GetFuturePosition(float time)
    {
        float futureAngle = _currentAngle + _direction * _currentRotationSpeed * time;
        float x = anchorPoint.position.x + radius * Mathf.Cos(futureAngle * Mathf.Deg2Rad);
        float y = anchorPoint.position.y + radius * Mathf.Sin(futureAngle * Mathf.Deg2Rad);
        return new Vector3(x, y, transform.position.z);
    }

    //Draw line from player to anchor point using linerenderer
    private void DrawLine()
    {

        if (!drawLine)
        {
            return;
        }



        if (_lineRenderer == null)
        {
            return;
        }

        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, anchorPoint.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, anchorPoint.position);
    }


}
