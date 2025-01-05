using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AbilityTestUIManager : MonoBehaviour
{
    [Header("Abilities")]
    [SerializeField] private BossAbilityTester bossAbilityTester;
    [SerializeField] private CircularSweepAttack circularSweepAttack;
    [SerializeField] private ConcentricShockwavesAttack concentricShockwavesAttack;


    [Header("Change Field To Fire UI")]
    [SerializeField] private Button changeFieldToFireButton;

    [Header("Fire Slash UI")]
    [SerializeField] private Button fireSlashButton;

    [Header("Circular Sweep UI")]
    [SerializeField] private Button startSweepButton;
    [SerializeField] private Slider startAngleSlider;
    [SerializeField] private TextMeshProUGUI startAngleValueLabel;
    [SerializeField] private Toggle isClockwiseToggle;
    [SerializeField] private Slider durationSlider;
    [SerializeField] private TextMeshProUGUI durationValueLabel;
    [SerializeField] private Slider speedSlider;
    [SerializeField] private TextMeshProUGUI speedValueLabel;

    [Header("Concentric Shockwaves UI")]
    [SerializeField] private Button startShockwavesButton;
    [SerializeField] private Slider shockwaveIntervalSlider;
    [SerializeField] private TextMeshProUGUI shockwaveIntervalValueLabel;
    [SerializeField] private Slider numberOfShockwavesSlider;
    [SerializeField] private TextMeshProUGUI numberOfShockwavesValueLabel;
    [SerializeField] private Slider shockwaveExpansionSpeedSlider;
    [SerializeField] private TextMeshProUGUI shockwaveExpansionSpeedValueLabel;
    [SerializeField] private Slider shockwaveDamageSlider;
    [SerializeField] private TextMeshProUGUI shockwaveDamageValueLabel;
    [SerializeField] private Slider numberOfSegmentsSlider;
    [SerializeField] private TextMeshProUGUI numberOfSegmentsValueLabel;
    [SerializeField] private TMP_InputField gapSegmentsInputField;
    [SerializeField] private Toggle useRandomGapsToggle;
    [SerializeField] private Slider numberOfGapsSlider;
    [SerializeField] private TextMeshProUGUI numberOfGapsValueLabel;
    [SerializeField] private Slider gapSizeSlider;
    [SerializeField] private TextMeshProUGUI gapSizeValueLabel;
    [SerializeField] private Slider shockwaveStartRadiusSlider;
    [SerializeField] private TextMeshProUGUI shockwaveStartRadiusValueLabel;
    [SerializeField] private Toggle shockwaveExpandOutwardToggle;

    [Header("Ghost Abilities UI")]
    [SerializeField] private Button spawnRadialGhostsButton;
    [SerializeField] private Button spawnLinearGhostsButton;
    [SerializeField] private Button spawnChasingGhostButton;

    [Header("Other Abilities UI")]
    [SerializeField] private Button throwSpearsButton;
    [SerializeField] private Button randomExplosionsButton;
    [SerializeField] private Button spawnRangedMinionsButton;

    private void Start()
    {
        // Add listeners to buttons and sliders
        changeFieldToFireButton.onClick.AddListener(OnChangeFieldToFireClicked);
        fireSlashButton.onClick.AddListener(OnFireSlashClicked);
        startSweepButton.onClick.AddListener(OnStartSweepClicked);
        startAngleSlider.onValueChanged.AddListener(OnStartAngleChanged);
        isClockwiseToggle.onValueChanged.AddListener(OnIsClockwiseChanged);
        durationSlider.onValueChanged.AddListener(OnDurationChanged);
        speedSlider.onValueChanged.AddListener(OnSpeedChanged);

        // Add listeners for Concentric Shockwaves UI
        startShockwavesButton.onClick.AddListener(OnStartShockwavesClicked);
        shockwaveIntervalSlider.onValueChanged.AddListener(OnShockwaveIntervalChanged);
        numberOfShockwavesSlider.onValueChanged.AddListener(OnNumberOfShockwavesChanged);
        shockwaveExpansionSpeedSlider.onValueChanged.AddListener(OnShockwaveExpansionSpeedChanged);
        shockwaveDamageSlider.onValueChanged.AddListener(OnShockwaveDamageChanged);
        numberOfSegmentsSlider.onValueChanged.AddListener(OnNumberOfSegmentsChanged);
        gapSegmentsInputField.onEndEdit.AddListener(OnGapSegmentsChanged);
        useRandomGapsToggle.onValueChanged.AddListener(OnUseRandomGapsChanged);
        numberOfGapsSlider.onValueChanged.AddListener(OnNumberOfGapsChanged);
        gapSizeSlider.onValueChanged.AddListener(OnGapSizeChanged);
        shockwaveStartRadiusSlider.onValueChanged.AddListener(OnShockwaveStartRadiusChanged);
        shockwaveExpandOutwardToggle.onValueChanged.AddListener(OnShockwaveExpandOutwardChanged);

        // Add listeners for Ghost Abilities UI
        spawnRadialGhostsButton.onClick.AddListener(OnSpawnRadialGhostsClicked);
        spawnLinearGhostsButton.onClick.AddListener(OnSpawnLinearGhostsClicked);
        spawnChasingGhostButton.onClick.AddListener(OnSpawnChasingGhostClicked);

        // Add listeners for Other Abilities UI
        throwSpearsButton.onClick.AddListener(OnThrowSpearsClicked);
        randomExplosionsButton.onClick.AddListener(OnRandomExplosionsClicked);
        spawnRangedMinionsButton.onClick.AddListener(OnSpawnRangedMinionsClicked);

        // Initialize UI element values
        UpdateStartAngleUI(0f);
        UpdateIsClockwiseUI(true);
        UpdateDurationUI(circularSweepAttack.GetDefaultLaserDuration());
        UpdateSpeedUI(circularSweepAttack.GetDefaultLaserRotationSpeed());

        // Initialize Concentric Shockwaves UI element values
        UpdateShockwaveIntervalUI(concentricShockwavesAttack.GetDefaultShockwaveInterval());
        UpdateNumberOfShockwavesUI(concentricShockwavesAttack.GetDefaultNumberOfShockwaves());
        UpdateShockwaveExpansionSpeedUI(concentricShockwavesAttack.GetDefaultShockwaveExpansionSpeed());
        UpdateShockwaveDamageUI(concentricShockwavesAttack.GetDefaultShockwaveDamage());
        UpdateNumberOfSegmentsUI(concentricShockwavesAttack.GetDefaultNumberOfSegments());
        UpdateGapSegmentsUI(concentricShockwavesAttack.GetDefaultGapSegments());
        UpdateUseRandomGapsUI(concentricShockwavesAttack.GetUseRandomGaps());
        UpdateNumberOfGapsUI(concentricShockwavesAttack.GetDefaultNumberOfGaps());
        UpdateGapSizeUI(concentricShockwavesAttack.GetDefaultGapSize());
        UpdateShockwaveStartRadiusUI(concentricShockwavesAttack.GetDefaultStartRadius());
        UpdateShockwaveExpandOutwardUI(concentricShockwavesAttack.GetExpandOutward());

        // Set slider min and max values
        durationSlider.minValue = 0f; 
        durationSlider.maxValue = circularSweepAttack.GetDefaultLaserDuration() * 2f;
        speedSlider.minValue = 0f;
        speedSlider.maxValue = circularSweepAttack.GetDefaultLaserRotationSpeed() * 10f;

        // Set Concentric Shockwaves slider min and max values
        shockwaveIntervalSlider.minValue = 0.1f;
        shockwaveIntervalSlider.maxValue = 10f;
        numberOfShockwavesSlider.minValue = 1;
        numberOfShockwavesSlider.maxValue = 10;
        shockwaveExpansionSpeedSlider.minValue = 1f;
        shockwaveExpansionSpeedSlider.maxValue = 20f;
        shockwaveDamageSlider.minValue = 1f;
        shockwaveDamageSlider.maxValue = 50f;
        numberOfSegmentsSlider.minValue = 10;
        numberOfSegmentsSlider.maxValue = 360;
        numberOfGapsSlider.minValue = 1;
        numberOfGapsSlider.maxValue = 10;
        gapSizeSlider.minValue = 1;
        gapSizeSlider.maxValue = 60;
        shockwaveStartRadiusSlider.minValue = 0f;
        shockwaveStartRadiusSlider.maxValue = 30f;

 
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
        float duration = durationSlider.value;
        float speed = speedSlider.value;

        // Use default values if sliders are at their minimum
        if (duration == durationSlider.minValue)
        {
            duration = circularSweepAttack.GetDefaultLaserDuration();
        }
        if (speed == speedSlider.minValue)
        {
            speed = circularSweepAttack.GetDefaultLaserRotationSpeed();
        }

        circularSweepAttack.StartCircularSweep(startAngle, isClockwise, duration, speed);
    }

    public void OnStartShockwavesClicked()
    {
        // Get the gap segments from the input field if not using random gaps
        if (!concentricShockwavesAttack.GetUseRandomGaps())
        {
            List<int> gapSegments = ParseGapSegmentsInput(gapSegmentsInputField.text);
            concentricShockwavesAttack.SetGapSegments(gapSegments);
        }

        concentricShockwavesAttack.StartConcentricShockwaves();
    }

    public void OnSpawnRadialGhostsClicked()
    {
        bossAbilityTester.spawnRadialGhostsAbility.SpawnGhosts();
    }

    public void OnSpawnLinearGhostsClicked()
    {
        bossAbilityTester.spawnLinearGhostsAbility.SpawnGhosts();
    }

    public void OnSpawnChasingGhostClicked()
    {
        bossAbilityTester.spawnChasingGhostAbility.SpawnGhost();
    }

    public void OnThrowSpearsClicked()
    {
        bossAbilityTester.throwSpearsAbility.ThrowSpears();
    }

    public void OnRandomExplosionsClicked()
    {
        bossAbilityTester.randomExplosionsAbility.TriggerExplosions();
    }

    public void OnSpawnRangedMinionsClicked()
    {
        bossAbilityTester.spawnRangedMinionsAbility.SpawnMinions();
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

    public void OnDurationChanged(float value)
    {
        UpdateDurationUI(value);
    }

    public void OnSpeedChanged(float value)
    {
        UpdateSpeedUI(value);
    }

    public void OnShockwaveIntervalChanged(float value)
    {
        concentricShockwavesAttack.SetShockwaveInterval(value);
        UpdateShockwaveIntervalUI(value);
    }

    public void OnNumberOfShockwavesChanged(float value)
    {
        concentricShockwavesAttack.SetNumberOfShockwaves((int)value);
        UpdateNumberOfShockwavesUI((int)value);
    }

    public void OnShockwaveExpansionSpeedChanged(float value)
    {
        concentricShockwavesAttack.SetShockwaveExpansionSpeed(value);
        UpdateShockwaveExpansionSpeedUI(value);
    }

    public void OnShockwaveDamageChanged(float value)
    {
        concentricShockwavesAttack.SetShockwaveDamage(value);
        UpdateShockwaveDamageUI(value);
    }

    public void OnNumberOfSegmentsChanged(float value)
    {
        concentricShockwavesAttack.SetNumberOfSegments((int)value);
        UpdateNumberOfSegmentsUI((int)value);
    }

    public void OnGapSegmentsChanged(string value)
    {
        List<int> gapSegments = ParseGapSegmentsInput(value);
        concentricShockwavesAttack.SetGapSegments(gapSegments);
        UpdateGapSegmentsUI(gapSegments);
    }

    public void OnUseRandomGapsChanged(bool value)
    {
        concentricShockwavesAttack.SetUseRandomGaps(value);
        UpdateUseRandomGapsUI(value);
    }

    public void OnNumberOfGapsChanged(float value)
    {
        concentricShockwavesAttack.SetNumberOfGaps((int)value);
        UpdateNumberOfGapsUI((int)value);
    }

    public void OnGapSizeChanged(float value)
    {
        concentricShockwavesAttack.SetGapSize((int)value);
        UpdateGapSizeUI((int)value);
    }

    public void OnShockwaveStartRadiusChanged(float value)
    {
        concentricShockwavesAttack.SetStartRadius(value);
        UpdateShockwaveStartRadiusUI(value);
    }

    public void OnShockwaveExpandOutwardChanged(bool value)
    {
        concentricShockwavesAttack.SetExpandOutward(value);
        UpdateShockwaveExpandOutwardUI(value);
    }

    // --- UI Update Methods ---

    private void UpdateStartAngleUI(float value)
    {
        startAngleValueLabel.text = "Start Angle: " + value.ToString("F0");
    }

    private void UpdateIsClockwiseUI(bool value)
    {
    }

    private void UpdateDurationUI(float value)
    {
        durationValueLabel.text = "Duration: " + value.ToString("F1");
    }

    private void UpdateSpeedUI(float value)
    {
        speedValueLabel.text = "Speed: " + value.ToString("F0");
    }

    private void UpdateShockwaveIntervalUI(float value)
    {
        shockwaveIntervalValueLabel.text = "Interval: " + value.ToString("F1");
    }

    private void UpdateNumberOfShockwavesUI(int value)
    {
        numberOfShockwavesValueLabel.text = "Number: " + value.ToString();
    }

    private void UpdateShockwaveExpansionSpeedUI(float value)
    {
        shockwaveExpansionSpeedValueLabel.text = "Speed: " + value.ToString("F1");
    }

    private void UpdateShockwaveDamageUI(float value)
    {
        shockwaveDamageValueLabel.text = "Damage: " + value.ToString("F1");
    }

    private void UpdateNumberOfSegmentsUI(int value)
    {
        numberOfSegmentsValueLabel.text = "Segments: " + value.ToString();
    }

    //This doesnt work properly, or rather it does but you would have to input a gazillion numbers
    private void UpdateGapSegmentsUI(List<int> gapSegments)
    {
        // Convert the list of integers to a comma-separated string for display
        string gapSegmentsString = string.Join(", ", gapSegments);
        gapSegmentsInputField.text = gapSegmentsString;
    }

    private void UpdateUseRandomGapsUI(bool value)
    {
        useRandomGapsToggle.isOn = value;
        gapSegmentsInputField.interactable = !value;
    }

    private void UpdateNumberOfGapsUI(int value)
    {
        numberOfGapsValueLabel.text = "# of Gaps: " + value.ToString();
    }

    private void UpdateGapSizeUI(int value)
    {
        gapSizeValueLabel.text = "Gap Size: " + value.ToString();
    }

    private void UpdateShockwaveStartRadiusUI(float value)
    {
        shockwaveStartRadiusValueLabel.text = "Start Radius: " + value.ToString("F1");
    }

    private void UpdateShockwaveExpandOutwardUI(bool value)
    {
        shockwaveExpandOutwardToggle.isOn = value;
    }

    // --- Helper Methods --- 
    private List<int> ParseGapSegmentsInput(string input)
    {
        List<int> gapSegments = new List<int>();
        string[] segments = input.Split(',');
        foreach (string segment in segments)
        {
            if (int.TryParse(segment.Trim(), out int segmentIndex))
            {
                gapSegments.Add(segmentIndex);
            }
        }
        return gapSegments;
    }
}