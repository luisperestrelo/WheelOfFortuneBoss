using UnityEngine;

public class TapToChangeDirection : IMovementScheme
{
    private PlayerSpinMovement _player;

    public void Initialize(PlayerSpinMovement player)
    {
        _player = player;
    }

    public void UpdateMovement()
    {
        // Handle input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Direction *= -1f;
        }

        _player.CurrentAngle += _player.Direction * _player.MaxRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
} 