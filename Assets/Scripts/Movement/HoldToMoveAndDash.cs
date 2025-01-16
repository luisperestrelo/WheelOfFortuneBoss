using UnityEngine;

public class HoldToMoveAndDash : IMovementScheme
{
    private PlayerSpinMovement _player;
    private bool _isDashing;
    private float _dashCooldown = 1f;
    private float _dashCooldownTimer;
    private float _dashDistance = 90f; //bigger distance and lower duration means faster dash
    private float _dashDuration = 0.1f;

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
        _isDashing = false;
        _dashCooldownTimer = 0f;
    }


    public void UpdateMovement()
    {
        // Handle dash input and cooldown
        //messy asf but we were using W and M2 in the other schemes... But in this one Space makes more sense for dash, so I kept all 3
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.W)) && !_isDashing && _dashCooldownTimer <= 0f)
        {
            _isDashing = true;
            _dashCooldownTimer = _dashCooldown;
            Dash();
        }

        if (_dashCooldownTimer > 0f)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }

        // Handle movement when not dashing
        if (!_isDashing)
        {
            if (Input.GetKey(KeyCode.E))
            {
                _player.Direction = -1f; // Counter-clockwise
                if (_player.UsesAcceleration)
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
                if (_player.UsesAcceleration)
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
                // No input, decelerate to a stop if using acceleration
                if (_player.UsesAcceleration)
                {
                    _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, 0f, _player.DecelerationRate * Time.deltaTime);
                }
                else
                { // instantly stop
                    _player.CurrentRotationSpeed = 0f;
                }
            }
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle = (_player.CurrentAngle % 360f + 360f) % 360f;
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
        _player.CurrentRotationSpeed = _player.Direction * _dashDistance / _dashDuration; // Adjust dash speed here 
    }
}