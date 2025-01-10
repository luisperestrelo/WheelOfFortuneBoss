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

            // Add cases for other field types here
            default:
                Debug.LogError("FieldEffectHandlerFactory::CreateEffectHandler: No handler found for field type " + fieldData.FieldType);
                return null;
        }
        }
} 