using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlSchemeDropdown : MonoBehaviour
{
    [SerializeField] private PlayerSpinMovement player;
    private TMP_Dropdown dropdown;
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }
    public void UpdateScheme()
    {
        switch (dropdown.value) {
            case 0:
                player.ChangeMovementScheme(PlayerSpinMovement.MovementSchemeType.TapToChangeFireToSlow);
                break;
            case 1:
                player.ChangeMovementScheme(PlayerSpinMovement.MovementSchemeType.HoldToMove);
                break;
            case 2:
                player.ChangeMovementScheme(PlayerSpinMovement.MovementSchemeType.TapToChangeDirection);
                break;
            default:
                break;
        }
    }
}
