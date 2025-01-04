using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityTestUIManager : MonoBehaviour
{
    [Header("Abilities")]
    public BossAbilityTester bossAbilityTester;
    public CircularSweepAttack circularSweepAttack;

    [Header("Change Field To Fire UI")]
    public Button changeFieldToFireButton;

    [Header("Fire Slash UI")]
    public Button fireSlashButton;

    [Header("Circular Sweep UI")]
    public Button startSweepButton;
    public Slider startAngleSlider;
    public TextMeshProUGUI startAngleValueLabel;
    public Toggle isClockwiseToggle;

    private void Start()
    {
        // Add listeners to buttons and sliders
        changeFieldToFireButton.onClick.AddListener(OnChangeFieldToFireClicked);
        fireSlashButton.onClick.AddListener(OnFireSlashClicked);
        startSweepButton.onClick.AddListener(OnStartSweepClicked);
        startAngleSlider.onValueChanged.AddListener(OnStartAngleChanged);
        isClockwiseToggle.onValueChanged.AddListener(OnIsClockwiseChanged);

        // Initialize UI element values
        UpdateStartAngleUI(0f); 
        UpdateIsClockwiseUI(true); 
    }

    // --- Button Click Handlers ---

    public void OnChangeFieldToFireClicked()
    {
        bossAbilityTester.ChangeRandomFieldToFire();
    }

    public void OnFireSlashClicked()
    {
        bossAbilityTester.FireSlash();
    }

    public void OnStartSweepClicked()
    {
        float startAngle = startAngleSlider.value;
        bool isClockwise = isClockwiseToggle.isOn;
        circularSweepAttack.StartCircularSweep(startAngle, isClockwise);
    }

    // --- Slider/Toggle Change Handlers ---

    public void OnStartAngleChanged(float value)
    {
        UpdateStartAngleUI(value);
    }

    public void OnIsClockwiseChanged(bool value)
    {
        UpdateIsClockwiseUI(value);
    }

    // --- UI Update Methods ---

    private void UpdateStartAngleUI(float value)
    {
        startAngleValueLabel.text = "Start Angle: " + value.ToString("F0");
    }

    private void UpdateIsClockwiseUI(bool value)
    {
       
    }
} 