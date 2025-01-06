using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: make the radius come from a CircularPath object attached to the boss
public class PlayerSpinMovement : MonoBehaviour
{
    [SerializeField] private Transform anchorPoint;
    //[SerializeField] private float defaultRotationSpeed = 180f; // degrees per second
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
    [SerializeField] private CircularPath circularPath;

    [Header("Movement Settings")]
    [SerializeField] private float maxRotationSpeed = 180f; // degrees per second (replaces defaultRotationSpeed)
    [SerializeField] private float accelerationRate = 1080f; // degrees per second squared (high value for quick acceleration)
    [SerializeField] private float decelerationRate = 720f; // degrees per second squared (for noticeable deceleration on direction change)

    [Header("Movement Scheme")]
    [SerializeField] private IMovementScheme currentMovementScheme;
    [SerializeField] private MovementSchemeType movementSchemeType;

    private enum MovementSchemeType
    {
        TapToChangeDirection,
        AccelerationBased,
        HoldToMove,
        TwoInputSpaceAndBoost,
        TwoInputSpaceAndBoostAccel,
        TwoInputSpaceAndSlow,
        TwoInputSpaceAndSlowAccel,
        TwoInputSpaceAndDash,
        TwoInputSpaceAndDashAccel,
        TwoInputSpaceAndStop,
        TwoInputSpaceAndMove,
        TwoInputSpaceAndToggleMove,
        HoldToMoveNoAccel,
        HoldToMoveAndDash
    }

    private float _currentAngle = 50f;
    public float CurrentAngle
    {
        get { return _currentAngle; }
        set { _currentAngle = value; }
    }

    private float _direction = 1f;
    public float Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    private float _currentRotationSpeed;
    public float CurrentRotationSpeed
    {
        get { return _currentRotationSpeed; }
        set { _currentRotationSpeed = value; }
    }

    public float MaxRotationSpeed
    {
        get { return maxRotationSpeed; }
    }

    public float AccelerationRate
    {
        get { return accelerationRate; }
    }

    public float DecelerationRate
    {
        get { return decelerationRate; }
    }

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        radius = circularPath.GetRadius();

        InitializeMovementScheme();
    }

    private void InitializeMovementScheme()
    {
        switch (movementSchemeType)
        {
            case MovementSchemeType.TapToChangeDirection:
                currentMovementScheme = new TapToChangeDirection();
                break;
            case MovementSchemeType.AccelerationBased:
                currentMovementScheme = new AccelerationBasedMovement();
                break;
            case MovementSchemeType.HoldToMove:
                currentMovementScheme = new HoldToMove();
                break;
            case MovementSchemeType.TwoInputSpaceAndBoost:
                currentMovementScheme = new TwoInputSpaceAndBoost();
                break;
            case MovementSchemeType.TwoInputSpaceAndBoostAccel:
                currentMovementScheme = new TwoInputSpaceAndBoostAccel();
                break;
            case MovementSchemeType.TwoInputSpaceAndSlow:
                currentMovementScheme = new TwoInputSpaceAndSlow();
                break;
            case MovementSchemeType.TwoInputSpaceAndSlowAccel:
                currentMovementScheme = new TwoInputSpaceAndSlowAccel();
                break;
            case MovementSchemeType.TwoInputSpaceAndDash:
                currentMovementScheme = new TwoInputSpaceAndDash();
                break;
            case MovementSchemeType.TwoInputSpaceAndDashAccel:
                currentMovementScheme = new TwoInputSpaceAndDashAccel();
                break;
            case MovementSchemeType.TwoInputSpaceAndStop:
                currentMovementScheme = new TwoInputSpaceAndStop();
                break;
            case MovementSchemeType.TwoInputSpaceAndMove:
                currentMovementScheme = new TwoInputSpaceAndMove();
                break;
            case MovementSchemeType.TwoInputSpaceAndToggleMove:
                currentMovementScheme = new TwoInputSpaceAndToggleMove();
                break;
            case MovementSchemeType.HoldToMoveNoAccel:
                currentMovementScheme = new HoldToMoveNoAccel();
                break;
            case MovementSchemeType.HoldToMoveAndDash:
                currentMovementScheme = new HoldToMoveAndDash();
                break;
            default:
                Debug.LogError("Invalid movement scheme type selected.");
                break;
        }
        

        currentMovementScheme.Initialize(this);
    }

    private void Update()
    {
        if (circularPath != null)
        {
            radius = circularPath.GetRadius();
        }

         currentMovementScheme.UpdateMovement();
/*
        if (Input.GetKeyDown(KeyCode.W) && Time.time > nextBoostTime)
        {
            StartCoroutine(SpeedBoostRoutine());
        }
        else if (Input.GetKeyDown(KeyCode.S) && Time.time > nextSlowTime)
        {
            StartCoroutine(SpeedSlowRoutine());
        } */

        DrawLine();

        if (anchorPoint == null)
        {
            return;
        }

        float x = anchorPoint.position.x + radius * Mathf.Cos(_currentAngle * Mathf.Deg2Rad);
        float y = anchorPoint.position.y + radius * Mathf.Sin(_currentAngle * Mathf.Deg2Rad);

        transform.position = new Vector3(x, y, transform.position.z);
    }

/*     private IEnumerator SpeedBoostRoutine()
    {
        float oldSpeed = _currentRotationSpeed; // might be useful in a later version
        _currentRotationSpeed = boostAmount;

        nextBoostTime = Time.time + boostCooldown;

        yield return new WaitForSeconds(boostDuration);

        //_currentRotationSpeed = oldSpeed;
        _currentRotationSpeed = maxRotationSpeed;
    }

    private IEnumerator SpeedSlowRoutine()
    {
        float oldSpeed = _currentRotationSpeed;
        _currentRotationSpeed = slowAmount;

        nextSlowTime = Time.time + slowCooldown;

        yield return new WaitForSeconds(slowDuration);

        //_currentRotationSpeed = oldSpeed;
        _currentRotationSpeed = maxRotationSpeed;
    } */

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
