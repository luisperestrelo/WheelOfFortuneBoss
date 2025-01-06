using UnityEngine;

public class HoldToMoveAndDash : IMovementScheme
{
    private PlayerSpinMovement _player;
    private bool _isDashing;
    private float _dashCooldown = 1f; // Adjust as needed
    private float _dashCooldownTimer;
    private float _dashDistance = 120f; // Degrees to dash
    private float _dashDuration = 0.2f; // Duration of the dash in seconds
    private bool _applyAcceleration; // Flag to control acceleration

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
        _isDashing = false;
        _dashCooldownTimer = 0f;
        _applyAcceleration = true; // Enable acceleration by default
    }

    public void UpdateMovement()
    {
        // Handle dash input and cooldown
        if ((Input.GetKeyDown(KeyCode.Space)) && !_isDashing && _dashCooldownTimer <= 0f)
        {
            _isDashing = true;
            _dashCooldownTimer = _dashCooldown;
            _applyAcceleration = false; // Disable acceleration during dash
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
            // Handle input
            if (Input.GetKey(KeyCode.E))
            {
                _player.Direction = -1f; // Counter-clockwise
                if (_applyAcceleration)
                {
                    _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed, _player.AccelerationRate * Time.deltaTime);
                }
                else
                {
                    _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed;
                }
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                _player.Direction = 1f; // Clockwise
                if (_applyAcceleration)
                {
                    _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed, _player.AccelerationRate * Time.deltaTime);
                }
                else
                {
                    _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed;
                }
            }
            else
            {
                // No input, decelerate to a stop
                _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, 0f, _player.DecelerationRate * Time.deltaTime);
            }
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;

        // Reset dashing state after dash is complete
        if (_isDashing && Mathf.Abs(_player.CurrentAngle - _dashStartAngle) >= _dashDistance)
        {
            _isDashing = false;
            _applyAcceleration = true; // Re-enable acceleration after dash
        }
    }

    private float _dashStartAngle;

    private void Dash()
    {
        _dashStartAngle = _player.CurrentAngle;
        _player.CurrentRotationSpeed = _player.Direction * _dashDistance / _dashDuration;
    }
} 