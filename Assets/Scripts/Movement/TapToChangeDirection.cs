using UnityEngine;

public class TapToChangeDirection : IMovementScheme
{
    private PlayerSpinMovement _player;

    public string description = "Tap Space to change direction\nHold M1 to Fire";

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

        if (_player.UsesAcceleration)
        {
            _player.CurrentRotationSpeed = Mathf.MoveTowards(_player.CurrentRotationSpeed, _player.Direction * _player.MaxRotationSpeed, _player.AccelerationRate * Time.deltaTime);
        }
        else
        {
            _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed;
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle = (_player.CurrentAngle % 360f + 360f) % 360f; 
    }
} 