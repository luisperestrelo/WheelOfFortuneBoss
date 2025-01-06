using UnityEngine;

public class TwoInputSpaceAndDash : IMovementScheme
{
    private PlayerSpinMovement _player;
    private bool _isDashing;
    private float _dashCooldown = 1f; 
    private float _dashCooldownTimer;
    private float _dashDistance = 90f; 
    private float _dashDuration = 0.1f; 

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
        _isDashing = false;
        _dashCooldownTimer = 0f;
    }

    public void UpdateMovement()
    {
        // Handle input for changing direction
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Direction *= -1f;
        }

        // Handle dash input and cooldown
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(1)) && !_isDashing && _dashCooldownTimer <= 0f)
        {
            _isDashing = true;
            _dashCooldownTimer = _dashCooldown;
            Dash();
        }

        // Update dash cooldown timer
        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }

        // Handle movement when not dashing
        if (!_isDashing)
        {
            if (_player.UsesAcceleration)
            {
                // Accelerate or decelerate based on input
                float targetSpeed = _player.Direction * _player.MaxRotationSpeed;
                _player.CurrentRotationSpeed = Mathf.MoveTowards(
                    _player.CurrentRotationSpeed,
                    targetSpeed,
                    Mathf.Abs(_player.CurrentRotationSpeed) < Mathf.Abs(targetSpeed)
                        ? _player.AccelerationRate * Time.deltaTime
                        : _player.DecelerationRate * Time.deltaTime
                );
            }
            else
            {
                // No acceleration, directly set the speed
                _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed;
            }
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;

        // Reset dashing state after dash is complete
        if (_isDashing && Mathf.Abs(_player.CurrentAngle - _dashStartAngle) >= _dashDistance)
        {
            _isDashing = false;
        }
    }

    private float _dashStartAngle;

    private void Dash()
    {
        _dashStartAngle = _player.CurrentAngle;
        _player.CurrentRotationSpeed = _player.Direction * _dashDistance / _dashDuration; //adjust dash speed here
    }
} 