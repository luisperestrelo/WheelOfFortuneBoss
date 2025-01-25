using UnityEngine;
using System;

public static class FieldEffectHandlerFactory
{
    public static FieldEffectHandler CreateEffectHandler(Field fieldData)
    {
        switch (fieldData.FieldType)
        {
            case FieldType.DamageBuff:
                var damageBuffHandler = new GameObject().AddComponent<DamageBuffEffectHandler>();
                damageBuffHandler.Initialize(fieldData);
                damageBuffHandler.name = "DamageBuffEffectHandler";
                return damageBuffHandler;
            case FieldType.Shield:
                var shieldHandler = new GameObject().AddComponent<ShieldEffectHandler>();
                shieldHandler.Initialize(fieldData);
                shieldHandler.name = "ShieldEffectHandler";
                return shieldHandler;
            case FieldType.Fireball:
                var fireballHandler = new GameObject().AddComponent<FireballAttackEffectHandler>();
                fireballHandler.Initialize(fieldData);
                fireballHandler.name = "FireballAttackEffectHandler";
                return fireballHandler;
            case FieldType.FanOfKnives:
                var fanOfKnivesHandler = new GameObject().AddComponent<FanOfKnivesEffectHandler>();
                fanOfKnivesHandler.Initialize(fieldData);
                fanOfKnivesHandler.name = "FanOfKnivesEffectHandler";
                return fanOfKnivesHandler;
            case FieldType.LightningBlast:
                var lightningBlastHandler = new GameObject().AddComponent<LightningBlastEffectHandler>();
                lightningBlastHandler.Initialize(fieldData);
                lightningBlastHandler.name = "LightningBlastEffectHandler";
                return lightningBlastHandler;
            case FieldType.Heal:
                var healingHandler = new GameObject().AddComponent<HealingEffectHandler>();
                healingHandler.Initialize(fieldData);
                healingHandler.name = "HealingEffectHandler";
                return healingHandler;
            case FieldType.DamagePlayer:
                var damagePlayerHandler = new GameObject().AddComponent<DamagePlayerEffectHandler>();
                damagePlayerHandler.Initialize(fieldData);
                damagePlayerHandler.name = "DamagePlayerEffectHandler";
                return damagePlayerHandler;
            case FieldType.EldritchBlast:
                var eldritchBlastHandler = new GameObject().AddComponent<EldritchBlastEffectHandler>();
                eldritchBlastHandler.Initialize(fieldData);
                eldritchBlastHandler.name = "EldritchBlastEffectHandler";
                return eldritchBlastHandler;
            case FieldType.VoidBurst:
                var voidBurstHandler = new GameObject().AddComponent<VoidBurstEffectHandler>();
                voidBurstHandler.Initialize(fieldData);
                voidBurstHandler.name = "VoidBurstEffectHandler";
                return voidBurstHandler;
            case FieldType.DamagingField:
                var damagingFieldHandler = new GameObject().AddComponent<DamagingFieldEffectHandler>();
                damagingFieldHandler.Initialize(fieldData);
                damagingFieldHandler.name = "DamagingFieldEffectHandler";
                return damagingFieldHandler;
            case FieldType.ChargedVoidBurst:
                var chargedVoidBurstHandler = new GameObject().AddComponent<ChargedVoidBurstEffectHandler>();
                chargedVoidBurstHandler.Initialize(fieldData);
                chargedVoidBurstHandler.name = "ChargedVoidBurstEffectHandler";
                return chargedVoidBurstHandler;
            case FieldType.TentacleShieldBusterField:
                return new GameObject("TentacleShieldBusterEffectHandler").AddComponent<TentacleShieldBusterEffectHandler>();
            case FieldType.Stingshot:
                var stingshotHandler = new GameObject().AddComponent<StingshotAttackEffectHandler>();
                stingshotHandler.Initialize(fieldData);
                stingshotHandler.name = "StingshotAttackEffectHandler";
                return stingshotHandler;


            default:
                Debug.LogError("FieldEffectHandlerFactory::CreateEffectHandler: No handler found for field type " + fieldData.FieldType);
                return null;
        }
    }
} 