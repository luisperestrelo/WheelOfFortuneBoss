using UnityEngine;
using TMPro;

public class DebugUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private BossController bossController;

    private void Update()
    {
        if (bossController != null && stateText != null)
        {
            stateText.text = "Current State: " + bossController.GetStateMachine().currentState.GetType().Name;

            if (timerText != null)
            {
                timerText.text = "Timer: " + bossController.GetStateMachine().currentState.GetTimer().ToString("F2");
            }
        }
    }
} 