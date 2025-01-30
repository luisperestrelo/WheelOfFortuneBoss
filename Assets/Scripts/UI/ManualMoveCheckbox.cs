using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManualMoveCheckbox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI useQEToMovePrompt;
    private Toggle toggle;

    private PlayerSpinMovement movement;

    public void UpdateUI()
    {
        FindPlayerMovement();
        if(movement)
            toggle.isOn = movement.GetCurrentMovementScheme() == PlayerSpinMovement.MovementSchemeType.HoldToMove;
    }

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        toggle.onValueChanged.AddListener(UpdateScheme);

        UpdateUI();
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void FindPlayerMovement()
    {
        // last minute fix for the bug, when toggling Manual Move after the player died (movement doesnt exist anymore)
        if(!movement)
            movement = FindObjectOfType<PlayerSpinMovement>();
  
    }

    public void UpdateScheme(bool doManualMove)
    {
        if (!movement) FindPlayerMovement();

        if (movement)
        {
            if (doManualMove)
                movement.ChangeMovementScheme(PlayerSpinMovement.MovementSchemeType.HoldToMove);
            else
                movement.ChangeMovementScheme(PlayerSpinMovement.MovementSchemeType.TapToChangeFireToSlow);

            useQEToMovePrompt.gameObject.SetActive(doManualMove);
        }
        else
        {
            Debug.LogWarning("PlayerSpinMovement not found!");
        }
    }
}