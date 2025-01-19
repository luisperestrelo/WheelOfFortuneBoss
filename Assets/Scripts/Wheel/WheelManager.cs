using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class WheelManager : MonoBehaviour
{
    public List<WheelSegment> Segments { get;  set; } = new List<WheelSegment>();
    [SerializeField] private CircularPath circularPath;
    public CircularPath CircularPath => circularPath;

    public List<Field> FieldsToAddToWheel;

    public Material LineMaterial;
    public float LineWidth = 0.1f;


    [SerializeField] private GameObject chargeIndicatorPrefab;
    public Canvas indicatorCanvas; // fuck it we make it public . i guess TODO: fix this

    private void Awake()
    {
        InitializeWheel(FieldsToAddToWheel);
        DontDestroyOnLoad(gameObject);
    }

    public void AddField(Field field)
    {
        FieldsToAddToWheel.Add(field);

        AddFieldToSegments(field);
        UpdateWheelVisuals();
    }

    public void AddField(Field field, int index)
    {
        FieldsToAddToWheel.Insert(index, field);

        AddFieldToSegments(field, index);
        UpdateWheelVisuals();
    }

    public int AddFieldAtRandomIndex(Field field) //returns the index of the field
    {
        int randomIndex = Random.Range(0, FieldsToAddToWheel.Count);
        AddField(field, randomIndex);
        return randomIndex;
    }

    public int AddFieldAtRandomIndexExcept(Field field, int excludedIndex)
    {
        if (FieldsToAddToWheel.Count <= 1)
        {
            // If there's only one or zero fields, we can't exclude anything. Just a check that should never happen
            return AddFieldAtRandomIndex(field);
        }

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, FieldsToAddToWheel.Count);
        } while (randomIndex == excludedIndex);

        AddField(field, randomIndex);
        return randomIndex;
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

        yield return new WaitForSeconds(duration);

        // Restore the original field
        RestoreOriginalField(index, originalField);
    }

    private void RestoreOriginalField(int index, Field originalField)
    {
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

        // Clear existing segments
        foreach (WheelSegment segment in Segments)
        {
            Destroy(segment.gameObject);
        }
        Segments.Clear();

        // Calculate total relative size of all fields
        float totalRelativeSize = 0;
        foreach (Field f in fields)
        {
            totalRelativeSize += f.Size;
        }

        // Calculate segment angles based on relative size
        float currentAngle = 0;
        for (int i = 0; i < fields.Count; i++)
        {
            float fieldRelativeSize = fields[i].Size;
            float startAngle = currentAngle;
            float angleSize = (fieldRelativeSize / totalRelativeSize) * 360f;
            float endAngle = currentAngle + angleSize;

            // Create a new GameObject for the segment
            GameObject segmentObject = new GameObject($"Segment_{fields[i].FieldName}");
            segmentObject.transform.SetParent(transform);

            // Add the WheelSegment component and initialize it
            WheelSegment segment = segmentObject.AddComponent<WheelSegment>();
            segment.Initialize(fields[i], startAngle, endAngle);
            Segments.Add(segment);

            currentAngle = endAngle;
        }

        // Initialize EffectHandlers for each segment
        foreach (WheelSegment segment in Segments)
        {
            if (segment.EffectHandler != null)
            {
                segment.EffectHandler.Initialize(segment.Field);
                segment.EffectHandler.SetSegment(segment);
            }
        }

        DrawWheelLines();
        CreateCooldownTexts();
        CreateFieldIcons();
        CreateChargeIndicators();
    }

    private void CleanUpVisuals()
    {
        // Destroy the old lines
        foreach (Transform child in transform)
        {
            if (child.name.StartsWith("SegmentLine_"))
            {
                Destroy(child.gameObject);
            }

            if (child.name.StartsWith("CooldownText_"))
            {
                Destroy(child.gameObject);
            }

            if (child.name.StartsWith("FieldIcon_")) //not needed anymore i think
            {
                Destroy(child.gameObject);
            }
        }

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
        float radius = CircularPath.GetRadius() + 1.2f;

        // Draw lines only for the current segments
        for (int i = 0; i < Segments.Count; i++)
        {
            // Calculate start and end points of the segment lines
            // Subtract 90 degrees to correct the offset
            Vector3 startPoint = center + Quaternion.Euler(0, 0, Segments[i].StartAngle - 90) * Vector3.up * radius;

            GameObject lineObject = new GameObject($"SegmentLine_{Segments[i].StartAngle}");
            lineObject.transform.SetParent(transform);

            // Add a LineRenderer component to the new GameObject
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.material = LineMaterial;
            lineRenderer.startWidth = LineWidth;
            lineRenderer.endWidth = LineWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, center);
            lineRenderer.SetPosition(1, startPoint);
            lineRenderer.sortingLayerName = "Ground";
            lineRenderer.sortingOrder = 3;
            
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

        return null; // Should never happen, since the whole wheel is used, player should always be in a segment
    }

    private void CreateCooldownTexts()
    {
        foreach (WheelSegment segment in Segments)
        {
            GameObject textObject = new GameObject($"CooldownText_{segment.Field.FieldName}");
            textObject.transform.SetParent(transform);

            TextMesh textMesh = textObject.AddComponent<TextMesh>();
            textMesh.text = ""; // Initially empty
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.fontSize = 12;
            textMesh.color = Color.white;

            Vector3 center = CircularPath.GetCenter();
            float radius = CircularPath.GetRadius();
            float angle = (segment.StartAngle + segment.EndAngle) / 2f - 90;
            Vector3 textPosition = center + Quaternion.Euler(0, 0, angle) * Vector3.up * (radius + 0.5f); // Adjust 0.5f to position the text further out
            textObject.transform.position = textPosition;

            segment.CooldownText = textMesh;
        }
    }

    private void CreateFieldIcons()
    {
        foreach (WheelSegment segment in Segments)
        {
            // Set the segment's position and rotation instead of creating a separate icon
            Vector3 center = CircularPath.GetCenter();
            float radius = CircularPath.GetRadius();
            float angle = (segment.StartAngle + segment.EndAngle) / 2f - 90;
            Vector3 segmentPosition = center + Quaternion.Euler(0, 0, angle) * Vector3.up * (radius - 2.5f);
            segment.transform.position = segmentPosition;
            segment.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void CreateChargeIndicators()
    {
        if (chargeIndicatorPrefab == null || indicatorCanvas == null) return;

        foreach (WheelSegment segment in Segments)
        {
            if (segment.Field is ChargeableField)
            {
                // Instantiate the ChargeIndicator prefab as a child of a Canvas used just for this
                // Later we can use other ways to show the charge going up
                GameObject indicator = Instantiate(chargeIndicatorPrefab, indicatorCanvas.transform);

                // Set the position and rotation of the indicator to match the segment
                float angle = (segment.StartAngle + segment.EndAngle) / 2f;

                // We're doing some magic here that I found on the internet it works out alright
                Vector3 indicatorWorldPosition = CircularPath.GetCenter() + Quaternion.Euler(0, 0, angle - 90) * (CircularPath.GetRadius() * Vector3.up * 0.75f);


                Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, indicatorWorldPosition);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(indicatorCanvas.transform as RectTransform, screenPos, indicatorCanvas.worldCamera, out Vector2 canvasPos);

                indicator.transform.localPosition = canvasPos;
                indicator.transform.localRotation = Quaternion.Euler(0, 0, angle);

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
        // Calculate total relative size of all fields
        float totalRelativeSize = 0;
        foreach (Field f in FieldsToAddToWheel)
        {
            totalRelativeSize += f.Size;
        }

        // Calculate segment angles based on relative size
        float currentAngle = 0;
        for (int i = 0; i < FieldsToAddToWheel.Count; i++)
        {
            float fieldRelativeSize = FieldsToAddToWheel[i].Size;
            float startAngle = currentAngle;
            float angleSize = (fieldRelativeSize / totalRelativeSize) * 360f;
            float endAngle = currentAngle + angleSize;

            // Create OR  update segment
            if (i == FieldsToAddToWheel.Count - 1)
            {
                GameObject segmentObject = new GameObject($"Segment_{field.FieldName}");
                segmentObject.transform.SetParent(transform);

                WheelSegment segment = segmentObject.AddComponent<WheelSegment>();
                segment.Initialize(field, startAngle, endAngle);
                Segments.Add(segment);
            }
            else
            {
                Segments[i].StartAngle = startAngle;
                Segments[i].EndAngle = endAngle;
            }

            currentAngle = endAngle;
        }

        WheelSegment newSegment = Segments[Segments.Count - 1];
        if (newSegment.EffectHandler != null)
        {
            newSegment.EffectHandler.Initialize(newSegment.Field);
            newSegment.EffectHandler.SetSegment(newSegment);
        }
    }

    //override to choose position.
    private void AddFieldToSegments(Field field, int index)
    {
        // Calculate total relative size of all fields
        float totalRelativeSize = 0;
        foreach (Field f in FieldsToAddToWheel)
        {
            totalRelativeSize += f.Size;
        }

        // Calculate segment angles based on relative size
        float currentAngle = 0;
        for (int i = 0; i < FieldsToAddToWheel.Count; i++)
        {
            float fieldRelativeSize = FieldsToAddToWheel[i].Size;
            float startAngle = currentAngle;
            float angleSize = (fieldRelativeSize / totalRelativeSize) * 360f;
            float endAngle = currentAngle + angleSize;

            // Create OR  update segment
            if (i == index)
            {
                GameObject segmentObject = new GameObject($"Segment_{field.FieldName}");
                segmentObject.transform.SetParent(transform);

                WheelSegment segment = segmentObject.AddComponent<WheelSegment>();
                segment.Initialize(field, startAngle, endAngle);
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

            currentAngle = endAngle;
        }

        WheelSegment newSegment = Segments[index];
        if (newSegment.EffectHandler != null)
        {
            newSegment.EffectHandler.Initialize(newSegment.Field);
            newSegment.EffectHandler.SetSegment(newSegment);
        }
    }

    private void ChangeSegmentSize(int index, float newSize)
    {
        //TODO 
        // We can have upgrades that change the size of a field

    }

    //TODO: this doesn't care about relative sizes of fields, we need to fix that, but not a problem yet
    private void RemoveFieldFromSegments(int index)
    {
        Destroy(Segments[index].gameObject);

        // Clean up the EffectHandler for the removed segment
        if (Segments[index].EffectHandler != null)
        {
            Destroy(Segments[index].EffectHandler.gameObject);
        }

        Segments.RemoveAt(index);

        // Recalculate segment angles
        float anglePerSegment = 360f / Segments.Count;
        for (int i = 0; i < Segments.Count; i++)
        {
            Segments[i].StartAngle = i * anglePerSegment;
            Segments[i].EndAngle = (i + 1) * anglePerSegment;
           // Segments[i].UpdateSegment(Segments[i].StartAngle, Segments[i].EndAngle);
        }

        CleanUpVisuals();
        DrawWheelLines();
    }

    //Doesn't update sizes, this is more like to, "set a field on fire" and stuff like that
    private void ReplaceFieldInSegments(int index, Field newField)
    {
        Segments[index].Initialize(newField, Segments[index].StartAngle, Segments[index].EndAngle);

        if (Segments[index].EffectHandler != null)
        {
            Destroy(Segments[index].EffectHandler.gameObject);
        }

        Segments[index].Field = newField;
        Segments[index].EffectHandler = FieldEffectHandlerFactory.CreateEffectHandler(newField);
        if (Segments[index].EffectHandler != null)
        {
            Segments[index].EffectHandler.Initialize(newField);
            Segments[index].EffectHandler.SetSegment(Segments[index]);
        }
    }

    public void ReplaceFieldAndUpdateSizes(int index, Field newField)
    {
        if (index >= 0 && index < FieldsToAddToWheel.Count)
        {
            FieldsToAddToWheel[index] = newField;

            RemoveFieldFromSegments(index);

            // Add the new field at the same index, which will recalculate sizes
            AddFieldToSegments(newField, index);

            UpdateWheelVisuals();
        }
        else
        {
            Debug.LogError("WheelManager::ReplaceFieldAndUpdateSizes: Index out of range.");
        }
    }

    private void SwapSegments(int index1, int index2)
    {
        // Swap the segments in the list
        WheelSegment temp = Segments[index1];
        Segments[index1] = Segments[index2];
        Segments[index2] = temp;

        // Update the start and end angles, and positions for the swapped segments
        float anglePerSegment = 360f / Segments.Count;
        Segments[index1].StartAngle = index1 * anglePerSegment;
        Segments[index1].EndAngle = (index1 + 1) * anglePerSegment;
        Segments[index2].StartAngle = index2 * anglePerSegment;
        Segments[index2].EndAngle = (index2 + 1) * anglePerSegment;

        UpdateSegmentPositions(index1);
        UpdateSegmentPositions(index2);
    }

    private void UpdateSegmentPositions(int index)
    {
        WheelSegment segment = Segments[index];
        Vector3 center = CircularPath.GetCenter();
        float radius = CircularPath.GetRadius();
        float angle = (segment.StartAngle + segment.EndAngle) / 2f - 90;
        Vector3 segmentPosition = center + Quaternion.Euler(0, 0, angle) * Vector3.up * (radius - 2.5f);
        segment.transform.position = segmentPosition;
        segment.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void UpdateWheelVisuals()
    {
        // Remove old visual elements before creating new ones
        CleanUpVisuals();

        // Redraw lines, update cooldown texts, and field icons, probably more stuff later
        DrawWheelLines();
        CreateCooldownTexts();
        CreateFieldIcons();
        CreateChargeIndicators();
    }





}