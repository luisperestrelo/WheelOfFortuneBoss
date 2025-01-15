using System.Collections.Generic;
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

    private List<UISegment> uiSegments = new List<UISegment>();
    private WheelManager wheelManager; 
    private int selectedIndex = -1; 
    private UIWheelMode currentMode;
    private Field newField; 
    private Card newCard; 

    public void Initialize(WheelManager wheelManager, Field newField, Card newCard, UIWheelMode mode)
    {
        this.wheelManager = wheelManager;
        this.newField = newField;
        this.newCard = newCard;
        currentMode = mode;
        selectedIndex = -1;
        UpdateWheelVisualization();
    }

    private void UpdateWheelVisualization()
    {
        // Clear existing segments and lines
        foreach (UISegment segment in uiSegments)
        {
            Destroy(segment.gameObject);
        }
        uiSegments.Clear();

        foreach (Transform child in SegmentsContainer)
        {
            if (child.CompareTag("SegmentLine"))
            {
                Destroy(child.gameObject);
            }
        }

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

            DrawSegmentLine(segments[i].StartAngle);

            uiSegments.Add(uiSegment);
        }

        DrawSegmentLine(segments[segments.Count - 1].EndAngle);
    }

    private void DrawSegmentLine(float angle)
    {
        // We create a new UI image for the line
        GameObject lineObject = new GameObject($"SegmentLine_{angle}", typeof(Image));
        lineObject.transform.SetParent(SegmentsContainer);
        lineObject.tag = "SegmentLine";

        RectTransform rectTransform = lineObject.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;


        //float wheelRadius = WheelImage.rectTransform.rect.width / 2f;
        float wheelRadius = 2000f;
        rectTransform.sizeDelta = new Vector2(wheelRadius, LineWidth);

        rectTransform.localRotation = Quaternion.Euler(0, 0, angle);

        rectTransform.pivot = new Vector2(0, 0.5f);

        Image lineImage = lineObject.GetComponent<Image>();
        lineImage.color = LineMaterial.color;
    }

    public void OnSegmentSelected(int index)
    {
        if (selectedIndex != -1)
        {
            uiSegments[selectedIndex].SegmentImage.color = Color.white;
        }

        selectedIndex = index;

        if (selectedIndex != -1)
        {
            uiSegments[selectedIndex].SegmentImage.color = Color.yellow;
        }

        Debug.Log($"Segment {index} selected");
    }

    public void ConfirmSelection()
    {
        if (selectedIndex == -1) return; // here we probably would show a message to the player that they need to select a field 

        if (currentMode == UIWheelMode.Replace)
        {
            wheelManager.ReplaceField(selectedIndex, newField);
            
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


}