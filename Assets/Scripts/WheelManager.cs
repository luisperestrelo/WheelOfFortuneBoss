using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class WheelManager : MonoBehaviour
{
    public List<WheelSegment> Segments { get; private set; } = new List<WheelSegment>();
    [SerializeField] private CircularPath circularPath;
    public CircularPath CircularPath => circularPath;

    public List<Field> FieldsToAddToWheel; // Assign your Field Scriptable Objects to this list in the Inspector

    public Material LineMaterial; // Assign a material for the lines in the Inspector
    public float LineWidth = 0.1f;

    [SerializeField] private GameObject chargeIndicatorPrefab; // Assign the ChargeIndicator prefab here
    [SerializeField] private Canvas indicatorCanvas; // Assign the Canvas here

    private void Start()
    {
        InitializeWheel(FieldsToAddToWheel);
    }

    public void AddField(Field field)
    {
        // Add the new field to the list of fields
        FieldsToAddToWheel.Add(field);

        // Update the wheel incrementally
        AddFieldToSegments(field);
        UpdateWheelVisuals();
    }

    public void AddField(Field field, int index)
    {
        // Add the new field to the list of fields at the specified index
        FieldsToAddToWheel.Insert(index, field);

        // Update the wheel incrementally
        AddFieldToSegments(field, index);
        UpdateWheelVisuals();
    }

    public void RemoveField(Field field)
    {
        int index = FieldsToAddToWheel.IndexOf(field);
        if (index != -1)
        {
            RemoveFieldFromSegments(index);
            FieldsToAddToWheel.RemoveAt(index);
            UpdateWheelVisuals();
        }
    }

    public void RemoveField(int index)
    {
        if (index >= 0 && index < FieldsToAddToWheel.Count)
        {
            RemoveFieldFromSegments(index);
            FieldsToAddToWheel.RemoveAt(index);
            UpdateWheelVisuals();
        }
    }

    public void ReplaceField(int index, Field newField)
    {
        if (index >= 0 && index < FieldsToAddToWheel.Count)
        {
            ReplaceFieldInSegments(index, newField);
            FieldsToAddToWheel[index] = newField;
            UpdateWheelVisuals();
        }
        else
        {
            Debug.LogError("WheelManager::ReplaceField: Index out of range.");
        }
    }

    public void ReplaceFieldTemporarily(int index, Field newField, float duration)
    {
        if (index >= 0 && index < FieldsToAddToWheel.Count)
        {
            StartCoroutine(ReplaceFieldTemporarilyCoroutine(index, newField, duration));
        }
        else
        {
            Debug.LogError("WheelManager::ReplaceFieldTemporarily: Index out of range.");
        }
    }

    private IEnumerator ReplaceFieldTemporarilyCoroutine(int index, Field newField, float duration)
    {
        // Store the original field and replace it with the new field
        Field originalField = FieldsToAddToWheel[index];
        ReplaceFieldInSegments(index, newField);
        FieldsToAddToWheel[index] = newField;
        UpdateWheelVisuals();

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Restore the original field
        RestoreOriginalField(index, originalField);
    }

    private void RestoreOriginalField(int index, Field originalField)
    {
        // Replace the temporary field with the original field
        ReplaceFieldInSegments(index, originalField);
        FieldsToAddToWheel[index] = originalField;
        UpdateWheelVisuals();
    }

    public void ClearWheel()
    {
        FieldsToAddToWheel.Clear();
        InitializeWheel(FieldsToAddToWheel);
    }

    public Field GetField(int index)
    {
        if (index >= 0 && index < FieldsToAddToWheel.Count)
        {
            return FieldsToAddToWheel[index];
        }
        else
        {
            Debug.LogError("WheelManager::GetField: Index out of range.");
            return null;
        }
    }

    public int GetIndexOfField(Field field)
    {
        return FieldsToAddToWheel.IndexOf(field);
    }

    public void SwapFields(int index1, int index2)
    {
        if (index1 >= 0 && index1 < FieldsToAddToWheel.Count && index2 >= 0 && index2 < FieldsToAddToWheel.Count)
        {
            // Swap fields in the list
            Field temp = FieldsToAddToWheel[index1];
            FieldsToAddToWheel[index1] = FieldsToAddToWheel[index2];
            FieldsToAddToWheel[index2] = temp;

            // Swap segments
            SwapSegments(index1, index2);
            UpdateWheelVisuals();
        }
        else
        {
            Debug.LogError("WheelManager::SwapFields: One or both indices are out of range.");
        }
    }

    public bool ContainsField(Field field)
    {
        return FieldsToAddToWheel.Contains(field);
    }

    public void InitializeWheel(List<Field> fields)
    {
        // Clean up existing visual elements before creating new ones
        CleanUpVisuals();
        CleanUpEffectHandlers();

        Segments.Clear();
        float anglePerSegment = 360f / fields.Count;
        for (int i = 0; i < fields.Count; i++)
        {
            float startAngle = i * anglePerSegment;
            float endAngle = (i + 1) * anglePerSegment;
            WheelSegment segment = new WheelSegment(fields[i], startAngle, endAngle);
            Segments.Add(segment);
        }

        foreach (WheelSegment segment in Segments)
        {
            if (segment.EffectHandler != null)
            {
                segment.EffectHandler.Initialize(segment.Field);

                // Always call SetSegment on the EffectHandler
                segment.EffectHandler.SetSegment(segment);
            }
        }

        DrawWheelLines();
        CreateCooldownTexts();
        CreateFieldIcons();
        CreateChargeIndicators(); // Call the new method
    }

    private void CleanUpVisuals()
    {
        // Destroy existing line objects
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("SegmentLine_") || child.name.StartsWith("CooldownText_") || child.name.StartsWith("FieldIcon_"))
            {
                Destroy(child.gameObject);
            }
        }

        // Destroy existing charge indicators (assuming they are children of the indicatorCanvas)
        if (indicatorCanvas != null)
        {
            foreach (Transform child in indicatorCanvas.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void DrawWheelLines()
    {
        if (CircularPath == null) return;

        Vector3 center = CircularPath.GetCenter();
        float radius = CircularPath.GetRadius();

        foreach (WheelSegment segment in Segments)
        {
            // Calculate start and end points of the segment lines
            // Subtract 90 degrees to correct the offset
            Vector3 startPoint = center + Quaternion.Euler(0, 0, segment.StartAngle - 90) * Vector3.up * radius;
            Vector3 endPoint = center + Quaternion.Euler(0, 0, segment.EndAngle - 90) * Vector3.up * radius;

            // Create a new GameObject for each line
            GameObject lineObject = new GameObject($"SegmentLine_{segment.StartAngle}");
            lineObject.transform.SetParent(transform); // Set the parent to the WheelManager

            // Add a LineRenderer component to the new GameObject
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.material = LineMaterial;
            lineRenderer.startWidth = LineWidth;
            lineRenderer.endWidth = LineWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, center);
            lineRenderer.SetPosition(1, startPoint);

            GameObject lineObject2 = new GameObject($"SegmentLine_{segment.EndAngle}");
            lineObject2.transform.SetParent(transform); // Set the parent to the WheelManager

            LineRenderer lineRenderer2 = lineObject2.AddComponent<LineRenderer>();
            lineRenderer2.material = LineMaterial;
            lineRenderer2.startWidth = LineWidth;
            lineRenderer2.endWidth = LineWidth;
            lineRenderer2.positionCount = 2;
            lineRenderer2.SetPosition(0, center);
            lineRenderer2.SetPosition(1, endPoint);
        }
    }

    public WheelSegment GetCurrentSegment(float playerAngle)
    {
        // Normalize the angle to be within 0-360 range
        playerAngle = (playerAngle % 360 + 360) % 360;

        foreach (WheelSegment segment in Segments)
        {
            // Check if the player's angle falls within the segment's angle range
            if (playerAngle >= segment.StartAngle && playerAngle < segment.EndAngle)
            {
                return segment;
            }
            // Special handling for segments that cross the 0/360 boundary
            else if (segment.EndAngle < segment.StartAngle && (playerAngle >= segment.StartAngle || playerAngle < segment.EndAngle))
            {
                return segment;
            }
        }

        return null; // Should ideally never happen if the wheel is set up correctly
    }

    private void CreateCooldownTexts()
    {
        foreach (WheelSegment segment in Segments)
        {
            // Create a new GameObject for the cooldown text
            GameObject textObject = new GameObject($"CooldownText_{segment.Field.FieldName}");
            textObject.transform.SetParent(transform); // Set the parent to the WheelManager

            // Add a TextMesh component (or TextMeshPro if you're using that)
            TextMesh textMesh = textObject.AddComponent<TextMesh>();
            textMesh.text = ""; // Initially empty
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.fontSize = 12;
            textMesh.color = Color.white;

            // Position the text above the segment
            Vector3 center = CircularPath.GetCenter();
            float radius = CircularPath.GetRadius();
            float angle = (segment.StartAngle + segment.EndAngle) / 2f - 90;
            Vector3 textPosition = center + Quaternion.Euler(0, 0, angle) * Vector3.up * (radius + 0.5f); // Adjust 0.5f to position the text further out
            textObject.transform.position = textPosition;

            // Store a reference to the text object in the segment
            segment.CooldownText = textMesh;
        }
    }

    private void CreateFieldIcons()
    {
        foreach (WheelSegment segment in Segments)
        {
            GameObject fieldIcon = new GameObject($"FieldIcon_{segment.Field.FieldName}");
            fieldIcon.transform.SetParent(transform);
            fieldIcon.transform.localPosition = Vector3.zero;
            fieldIcon.AddComponent<SpriteRenderer>().sprite = segment.Field.Icon;

            Vector3 center = CircularPath.GetCenter();
            float radius = CircularPath.GetRadius();
            float angle = (segment.StartAngle + segment.EndAngle) / 2f - 90;
            Vector3 iconPosition = center + Quaternion.Euler(0, 0, angle) * Vector3.up * (radius - 2.5f);
            fieldIcon.transform.position = iconPosition;
        }
    }

    private void CreateChargeIndicators()
    {
        if (chargeIndicatorPrefab == null || indicatorCanvas == null) return;

        foreach (WheelSegment segment in Segments)
        {
            if (segment.Field is ChargeableField)
            {
                // Instantiate the ChargeIndicator prefab as a child of the Canvas
                GameObject indicator = Instantiate(chargeIndicatorPrefab, indicatorCanvas.transform);

                // Set the position and rotation of the indicator to match the segment
                float angle = (segment.StartAngle + segment.EndAngle) / 2f;

                // Calculate the position in world space
                Vector3 indicatorWorldPosition = CircularPath.GetCenter() + Quaternion.Euler(0, 0, angle - 90) * (CircularPath.GetRadius() * Vector3.up * 0.75f);

                // Convert world space position to canvas space
                // Use RectTransformUtility.WorldToScreenPoint to convert to screen space first
                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, indicatorWorldPosition);

                // Then convert screen space to canvas space
                RectTransformUtility.ScreenPointToLocalPointInRectangle(indicatorCanvas.transform as RectTransform, screenPos, indicatorCanvas.worldCamera, out Vector2 canvasPos);

                indicator.transform.localPosition = canvasPos;
                indicator.transform.localRotation = Quaternion.Euler(0, 0, angle);

                // Set the indicator's Image component
                Image indicatorImage = indicator.GetComponent<Image>();
                if (indicatorImage != null)
                {
                    // Pass the Image component to the segment's effect handler
                    if (segment.EffectHandler is ChargeableFieldEffectHandler)
                    {
                        ((ChargeableFieldEffectHandler)segment.EffectHandler).SetChargeIndicatorImage(indicatorImage);
                    }
                }
            }
        }
    }

    private void Update()
    {
        foreach (WheelSegment segment in Segments)
        {
            segment.UpdateCooldown(Time.deltaTime);

            // Update the cooldown text
            if (segment.CooldownText != null)
            {
                if (segment.IsOnCooldown)
                {
                    segment.CooldownText.text = "On Cooldown!";
                }
                else
                {
                    segment.CooldownText.text = "";
                }
            }
        }
    }

    private void CleanUpEffectHandlers()
    {
        // Destroy existing effect handler game objects
        foreach (WheelSegment segment in Segments)
        {
            if (segment.EffectHandler != null)
            {
                Destroy(segment.EffectHandler.gameObject);
            }
        }
    }

    private void AddFieldToSegments(Field field)
    {
        // Calculate new segment angles
        float anglePerSegment = 360f / (FieldsToAddToWheel.Count);
        for (int i = 0; i < FieldsToAddToWheel.Count; i++)
        {
            float startAngle = i * anglePerSegment;
            float endAngle = (i + 1) * anglePerSegment;

            if (i == FieldsToAddToWheel.Count - 1)
            {
                // Create and add the new segment
                WheelSegment segment = new WheelSegment(field, startAngle, endAngle);
                Segments.Add(segment);
            }
            else
            {
                // Update existing segments
                Segments[i].StartAngle = startAngle;
                Segments[i].EndAngle = endAngle;
            }
        }

        // Initialize the EffectHandler for the new segment
        WheelSegment newSegment = Segments[Segments.Count - 1];
        if (newSegment.EffectHandler != null)
        {
            newSegment.EffectHandler.Initialize(newSegment.Field);
            newSegment.EffectHandler.SetSegment(newSegment);
        }
    }

    private void AddFieldToSegments(Field field, int index)
    {
        // Calculate new segment angles
        float anglePerSegment = 360f / FieldsToAddToWheel.Count;
        for (int i = 0; i < FieldsToAddToWheel.Count; i++)
        {
            float startAngle = i * anglePerSegment;
            float endAngle = (i + 1) * anglePerSegment;

            if (i == index)
            {
                // Create and insert the new segment
                WheelSegment segment = new WheelSegment(field, startAngle, endAngle);
                Segments.Insert(index, segment);
            }
            else if (i > index)
            {
                // Update segments after the inserted index
                Segments[i].StartAngle = startAngle;
                Segments[i].EndAngle = endAngle;
            }
            else
            {
                // Update segments before the inserted index
                Segments[i].StartAngle = startAngle;
                Segments[i].EndAngle = endAngle;
            }
        }

        // Initialize the EffectHandler for the new segment
        WheelSegment newSegment = Segments[index];
        if (newSegment.EffectHandler != null)
        {
            newSegment.EffectHandler.Initialize(newSegment.Field);
            newSegment.EffectHandler.SetSegment(newSegment);
        }
    }

    private void RemoveFieldFromSegments(int index)
    {
        // Clean up the EffectHandler for the removed segment
        if (Segments[index].EffectHandler != null)
        {
            Destroy(Segments[index].EffectHandler.gameObject);
        }

        // Remove the segment
        Segments.RemoveAt(index);

        // Recalculate segment angles
        float anglePerSegment = 360f / FieldsToAddToWheel.Count;
        for (int i = 0; i < Segments.Count; i++)
        {
            Segments[i].StartAngle = i * anglePerSegment;
            Segments[i].EndAngle = (i + 1) * anglePerSegment;
        }
    }

    private void ReplaceFieldInSegments(int index, Field newField)
    {
        // Clean up the EffectHandler for the replaced segment
        if (Segments[index].EffectHandler != null)
        {
            Destroy(Segments[index].EffectHandler.gameObject);
        }

        // Replace the segment's field and reinitialize its EffectHandler
        Segments[index].Field = newField;
        Segments[index].EffectHandler = FieldEffectHandlerFactory.CreateEffectHandler(newField);
        if (Segments[index].EffectHandler != null)
        {
            Segments[index].EffectHandler.Initialize(newField);
            Segments[index].EffectHandler.SetSegment(Segments[index]);
        }
    }

    private void SwapSegments(int index1, int index2)
    {
        // Swap the segments in the list
        WheelSegment temp = Segments[index1];
        Segments[index1] = Segments[index2];
        Segments[index2] = temp;

        // Update the start and end angles for the swapped segments
        float anglePerSegment = 360f / Segments.Count;
        Segments[index1].StartAngle = index1 * anglePerSegment;
        Segments[index1].EndAngle = (index1 + 1) * anglePerSegment;
        Segments[index2].StartAngle = index2 * anglePerSegment;
        Segments[index2].EndAngle = (index2 + 1) * anglePerSegment;
    }

    private void UpdateWheelVisuals()
    {
        // Redraw lines, update cooldown texts, and field icons
        CleanUpVisuals();

        DrawWheelLines();
        CreateCooldownTexts();
        CreateFieldIcons();
        CreateChargeIndicators();
    }

    // Add methods for bosses to modify the wheel here
}