using UnityEngine;

public class TwoInputSpaceAndMove : IMovementScheme
{
    private PlayerSpinMovement _player;

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
    }

    public void UpdateMovement()
    {
        // Handle input for changing direction
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Direction *= -1f;
        }

        // Handle input for moving
        if (Input.GetKey(KeyCode.W) || Input.GetMouseButton(1)) // Right-click
        {
            // Accelerate to normal speed
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
            // Decelerate to a stop
            if (_player.UsesAcceleration)
            {
                _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, 0f, _player.DecelerationRate * Time.deltaTime);
            }
            else
            {
                _player.CurrentRotationSpeed = 0f;
            }
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
}
