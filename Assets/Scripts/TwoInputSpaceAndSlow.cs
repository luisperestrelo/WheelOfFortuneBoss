using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoInputSpaceAndSlow : IMovementScheme
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

        // Handle input for slowing down
        if (Input.GetKey(KeyCode.W) || Input.GetMouseButton(1)) // Right-click
        {
            _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed * 0.5f; // Instant slow
        }
        else
        {
            _player.CurrentRotationSpeed = _player.Direction * _player.MaxRotationSpeed; // Instant return to normal speed
        }

        _player.CurrentAngle += _player.CurrentRotationSpeed * Time.deltaTime;
        _player.CurrentAngle %= 360f;
    }
}
