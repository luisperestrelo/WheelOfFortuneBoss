using UnityEngine;
using System.Collections;

public class EldritchBlastEffectHandler : ChargeableFieldEffectHandler
{
    private float damageAmount;
    private GameObject eldritchBlastPrefab;
    private DamagingField damagingField;
    private float damagingFieldDuration;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);
        //damageAmount = ((EldritchBlastField)fieldData).DamageAmount;
        eldritchBlastPrefab = ((EldritchBlastField)fieldData).EldritchBlastPrefab;

        // Get the DamagingField and its duration from the EldritchBlastField
        damagingField = ((EldritchBlastField)fieldData).DamagingField;
        damagingFieldDuration = ((EldritchBlastField)fieldData).DamagingFieldDuration;
    }

    protected override void OnChargeComplete(Player player)
    {
        // Find the boss and instantiate the Eldritch Blast
        BossController boss = FindObjectOfType<BossController>();
        if (boss != null)
        {
            // Calculate the offset for the "Sprite" child object
            Vector3 offset = boss.transform.Find("Sprite").localPosition;

            // Instantiate the Eldritch Blast at the boss's position plus the offset
            GameObject blast = Instantiate(eldritchBlastPrefab, boss.transform.position + offset, Quaternion.identity);

            // Assuming the Eldritch Blast prefab has a script to handle damage, set the damage value
            // Example: blast.GetComponent<EldritchBlast>().SetDamage(damageAmount);

            // Start the cooldown on the segment
            //Segment.StartCooldown();

            // Replace the field with a damaging field temporarily
            StartCoroutine(ReplaceWithDamagingField(damagingField, damagingFieldDuration));
        }
        else
        {
            Debug.LogError("EldritchBlastEffectHandler::OnChargeComplete: Could not find Boss in the scene.");
        }
    }

    private IEnumerator ReplaceWithDamagingField(DamagingField damagingField, float duration)
    {
        // Get the WheelManager
        WheelManager wheelManager = FindObjectOfType<WheelManager>();
        if (wheelManager != null)
        {
            // Get the index of the current segment
            int segmentIndex = wheelManager.Segments.IndexOf(Segment);

            // Replace the segment's field temporarily
            wheelManager.ReplaceFieldTemporarily(segmentIndex, damagingField, duration);
        }
        else
        {
            Debug.LogError("EldritchBlastEffectHandler::ReplaceWithDamagingField: Could not find WheelManager in the scene.");
        }

        yield return null; // Add a yield return to complete the coroutine
    }
} 