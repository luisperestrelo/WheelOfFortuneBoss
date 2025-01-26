using UnityEngine;

public class HoldToMove : IMovementScheme
{
    private PlayerSpinMovement _player;

    public string description = "Hold Q to move clockwise, E to move counter-clockwise.\nHold M1 to Fire";

    public string Description { get { return description; } }

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
    }

    public void UpdateMovement()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (Input.GetMouseButton(0))
                _player.Direction = -0.5f;
            else
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
            if (Input.GetMouseButton(0))
                _player.Direction = 0.5f;
            else
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
            // No input, decelerate to a stop
            if (_player.UsesAcceleration)
            {
                _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, 0f, _player.DecelerationRate * Time.deltaTime);
            }
            else
            { // instantly stop
                _player.CurrentRotationSpeed = 0f;
            }
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle = (_player.CurrentAngle % 360f + 360f) % 360f; 
    }
} 