using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIWheelMode { Replace, Insert }

public class UIWheel : MonoBehaviour
{
    public GameObject SegmentPrefab;
    public Transform SegmentsContainer;
    public Image WheelImage;
    public int MaxSegments = 6;
    public Material LineMaterial; // kinda weird but we get the color from here for now
    public float LineWidth = 0.1f;
    public float IconOffset = 2.5f;
    public Button ConfirmButton;
    public Color HighlightColor = Color.yellow; 
    public Color HoverColor = Color.gray;
    public float HighlightedLineWidth = 10f;
    public CardDisplay incomingCardDisplay = null;
    public TextMeshProUGUI explanationText = null;
    public string explanationTextAddField = "Click a line to place the new field. ";
    // private string explanationTextAddField2 = "Click a" + " <color=green>" + "line" + "</color>" + " to place the new field.";
    public string explanationTextReplaceField = "You are at maximum fields! Click a field to replace it";

    [SerializeField] private AudioClip selectLineSfx;
    

    private List<UISegment> uiSegments = new List<UISegment>();
    private List<GameObject> uiLines = new List<GameObject>();
    private WheelManager wheelManager;
    private int selectedIndex = -1;
    private UIWheelMode currentMode;
    private Field newField;
    private Card newCard;
    private GameObject selectedLine = null;


    public void Initialize(WheelManager wheelManager, Field newField, Card newCard, UIWheelMode mode)
    {
        this.wheelManager = wheelManager;
        this.newField = newField;
        this.newCard = newCard;
        currentMode = mode;
        selectedIndex = -1;
        incomingCardDisplay.DisplayCard(newCard);
        gameObject.SetActive(true);
        UpdateWheelVisualization();
    }

    private void OnEnable()
    {
        ConfirmButton.interactable = false;
    }

    private void UpdateWheelVisualization()
    {
        // Clear existing segments, lines, 
        foreach (UISegment segment in uiSegments)
        {
            Destroy(segment.gameObject);
        }
        uiSegments.Clear();

        foreach (GameObject line in uiLines)
        {
            Destroy(line);
        }
        uiLines.Clear();

        // Create new segments based on the actual wheel, the one that we have in-game
        // TODO: Later I want to make sure this doesn't draw "temporary" segments, ie segments that are not the segments
        // the player chose, but for now it's fine.
        List<WheelSegment> segments = wheelManager.Segments;
        if (segments.Count == 0) return;

        float anglePerSegment = 360f / segments.Count;
        for (int i = 0; i < segments.Count; i++)
        {
            GameObject segmentObject = Instantiate(SegmentPrefab, SegmentsContainer);
            UISegment uiSegment = segmentObject.GetComponent<UISegment>();

            uiSegment.Initialize(i, segments[i].Field.Icon, this);

            float midAngle = (segments[i].StartAngle + segments[i].EndAngle) / 2f; //icon goes here

            
            float wheelRadius = WheelImage.rectTransform.rect.width / 2f;

            
            float iconDistance = wheelRadius * 0.35f; // .5f would be middle, we will have to fiddle iwth UI stuff later

            Vector3 iconPosition = Quaternion.Euler(0, 0, midAngle - 90f) * Vector3.up * iconDistance; 
            segmentObject.transform.localPosition = iconPosition;

            // Draw the segment lines
            GameObject line = DrawSegmentLine(segments[i].StartAngle, i);
            uiLines.Add(line);

            uiSegments.Add(uiSegment);
        }

        if (currentMode == UIWheelMode.Insert)
        {
            explanationText.text = explanationTextAddField;
        }
        else
        {
            explanationText.text = explanationTextReplaceField;
        }

        
        /* if (segments.Count != MaxSegments)
        {
            DrawSegmentLine(segments[segments.Count - 1].EndAngle, segments.Count - 1);
        } */


    }

    private GameObject DrawSegmentLine(float angle, int index)
    {
        GameObject lineObject = new GameObject($"SegmentLine_{angle}_{index}", typeof(Image), typeof(Button));
        lineObject.transform.SetParent(SegmentsContainer);
        lineObject.tag = "SegmentLine"; // not using anymore

        RectTransform rectTransform = lineObject.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;

        // Calculate the length of the line based on the wheel's radius
        //float wheelRadius = WheelImage.rectTransform.rect.width / 2f;
        float wheelRadius = 2000f;
        rectTransform.sizeDelta = new Vector2(wheelRadius, LineWidth);

        rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

        // Set the pivot to the base of the line for proper rotation
        rectTransform.pivot = new Vector2(0, 0.5f);

        Image lineImage = lineObject.GetComponent<Image>();
        lineImage.color = LineMaterial.color;

        // the line itself acts as a button, player clicks on it to select where to put the new field
        Button lineButton = lineObject.GetComponent<Button>();

        lineButton.colors = new ColorBlock
        {
            normalColor = LineMaterial.color,
            highlightedColor = HoverColor,
            pressedColor = HighlightColor,
            selectedColor = HighlightColor,
            disabledColor = LineMaterial.color,
            colorMultiplier = 1f,
            fadeDuration = 0.1f
            
        };

        lineButton.onClick.AddListener(() => OnLineClicked(index));

        // Disable button interaction in replacement mode
        if (currentMode == UIWheelMode.Replace)
        {
            lineButton.interactable = false;
        }

        return lineObject;
    }

    private void OnLineClicked(int index)
    {
        if (currentMode == UIWheelMode.Insert)
        {
            Debug.Log($"Line {index} clicked (insertion point)");
            selectedIndex = index;
            ConfirmButton.interactable = true;
            SFXPool.instance.PlaySound(selectLineSfx);
        }

        else
        {
            Debug.Log($"Line {index} clicked (but not in insertion mode)");
        }
    }

    public void OnSegmentSelected(int index)
    {
        if (currentMode == UIWheelMode.Replace)
        {
            if (selectedIndex != -1)
            {
                uiSegments[selectedIndex].SegmentImage.color = Color.white;
            }

            selectedIndex = index;

            if (selectedIndex != -1)
            {
                uiSegments[selectedIndex].SegmentImage.color = HighlightColor;
            }

            Debug.Log($"Segment {index} selected");
        }
        else
        {
            Debug.Log($"Insertion mode is on, ignoring segment selection");
        }
    }

    public void ConfirmSelection()
    {
        if (selectedIndex == -1) return; // here we probably would show a message to the player that they need to select a field 

        if (currentMode == UIWheelMode.Replace)
        {
            wheelManager.ReplaceFieldAndUpdateSizes(selectedIndex, newField);

        }
        else
        {
            wheelManager.AddField(newField, selectedIndex);
        }

        RunManager.Instance.OnMidFightFieldCardSelected(newCard);

        // When we replace a field, we *do not* remove the card from the run. I think that's fine, and we can even allow the player to
        // customize their wheel with "excess cards" they own at some point.

        gameObject.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        gameObject.SetActive(false);
    }


}