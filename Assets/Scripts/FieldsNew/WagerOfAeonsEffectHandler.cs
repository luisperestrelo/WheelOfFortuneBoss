using UnityEngine;

public class WagerOfAeonsEffectHandler : FieldEffectHandler
{
    [SerializeField] private WagerOfAeonsAttack wagerAttack; 
    private float upgradeTimer = 0f;
    private float vulnerabilityTimer = 0f;

    public override void Initialize(Field fieldData)
    {
        base.Initialize(fieldData);

        WagerOfAeonsField wagerField = (WagerOfAeonsField)fieldData; 
        wagerAttack = wagerField.WagerOfAeonsAttack; 
        if (wagerAttack != null) wagerAttack.ResetAttackLevel();
    }

    public override void OnEnter(Player player)
    {
        Debug.Log("Entering WagerOfAeons Field");
        if (player != null && wagerAttack != null)
        {
            player.GetComponent<PlayerCombat>().CurrentAttack = wagerAttack;
        }
    }

    public override void OnStay(Player player, float deltaTime)
    {
        if (wagerAttack == null || player == null) return;

        upgradeTimer += deltaTime;
        if (upgradeTimer >= wagerAttack.TimeToUpgrade)
        {
            upgradeTimer = 0f;
            wagerAttack.IncrementAttackLevel();  // will cap at 6 inside the script
        }

        vulnerabilityTimer += deltaTime;
        if (vulnerabilityTimer >= wagerAttack.TimeToApplyVulnerability)
        {
            vulnerabilityTimer = 0f;
            BuffManager manager = player.GetComponent<BuffManager>();
            if (manager != null)
            {
                manager.ApplyBuff(new VulnerabilityBuff(wagerAttack.VulnerabilityMultiplier * playerStats.PositiveNegativeFieldsEffectivenessMultiplier,
                                                        wagerAttack.VulnerabilityDuration));
            }
        }
    }

    public override void OnExit(Player player)
    {
        Debug.Log("Exiting WagerOfAeons Field");
        if (player != null)
        {
            player.GetComponent<PlayerCombat>().CurrentAttack = null;
        }
        // Reset the attack level
        if (wagerAttack != null)
        {
            wagerAttack.ResetAttackLevel();
        }
    }
} 