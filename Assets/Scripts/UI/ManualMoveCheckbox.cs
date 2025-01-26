using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManualMoveCheckbox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI useQEToMovePrompt;
    [SerializeField] private PlayerSpinMovement movement;
    private Toggle toggle;
    private void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(UpdateScheme);
    }

    public void UpdateScheme(bool doManualMove)
    {
        if (doManualMove)
            movement.ChangeMovementScheme(PlayerSpinMovement.MovementSchemeType.HoldToMove);
        else
            movement.ChangeMovementScheme(PlayerSpinMovement.MovementSchemeType.TapToChangeFireToSlow);

        useQEToMovePrompt.gameObject.SetActive(doManualMove);
    }
}
