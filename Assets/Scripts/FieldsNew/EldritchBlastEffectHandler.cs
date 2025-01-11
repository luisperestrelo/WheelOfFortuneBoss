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
        //TODO: We just blast the boss directly for now, but i wanna change this
        BossController boss = FindObjectOfType<BossController>();
        if (boss != null)
        {
            // Calculate the offset for the "Sprite" child object, kinda disgusting gotta figure this out
            Vector3 offset = boss.transform.Find("Sprite").localPosition;

            // Instantiate the Eldritch Blast at the boss's position plus the offset
            GameObject blast = Instantiate(eldritchBlastPrefab, boss.transform.position + offset, Quaternion.identity);


            // Since we replace it  completely, no point using cooldown. We could consider cooldown reduction stuff
            // to affect the duration of the damaging field though.
            //Segment.StartCooldown();

            // Rplace the fuild with a temporary dmg field
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
            int segmentIndex = wheelManager.Segments.IndexOf(Segment);

            wheelManager.ReplaceFieldTemporarily(segmentIndex, damagingField, duration);
        }
        else
        {
            Debug.LogError("EldritchBlastEffectHandler::ReplaceWithDamagingField: Could not find WheelManager in the scene.");
        }

        yield return null; 
    }
} 