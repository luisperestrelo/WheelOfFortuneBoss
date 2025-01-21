using UnityEngine;

public class TwoInputSpaceAndBoost : IMovementScheme
{
    private PlayerSpinMovement _player;

    public string description = "Tap Space to change direction.\nHold M1 to Fire\n Hold W or M2 to increase speed";
    public string Description { get { return description; } }

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
    }

    public void UpdateMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Direction *= -1f;
        }

        // Handle input for speed boost
        if (Input.GetKey(KeyCode.W) || Input.GetMouseButton(1)) 
        {
            if (_player.UsesAcceleration)
            {
                _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed * 2f, _player.AccelerationRate * Time.deltaTime);
            }
            else
            {
                _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed * 2f; // Instant boost
            }
        }
        else
        {
            if (_player.UsesAcceleration)
            {
                _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed, _player.DecelerationRate * Time.deltaTime);
            }
            else
            {
                _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed; // Instant return to normal speed
            }
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle = (_player.CurrentAngle % 360f + 360f) % 360f; 
    }
} 