using UnityEngine;
using UnityEngine.UI;

public class TentacleShieldBusterEffectHandler : ChargeableFieldEffectHandler
{
    private MorsoraBossController bossController;

    public override void Initialize(Field fieldData)
    {

        base.Initialize(fieldData); // we could remove this to avoid player stat scaling if we had any, and also to change sound maybe


        bossController = FindObjectOfType<MorsoraBossController>();
    }

    public override void OnEnter(Player player)
    {
        base.OnEnter(player);
    }

    public override void OnStay(Player player, float deltaTime)
    {
        if (isCharging)
        {
            currentChargeTime += deltaTime; // * chargeUpSpeedMultiplier; 
            if (currentChargeTime >= chargeTime)
            {
                OnChargeComplete(player);
                currentChargeTime = 0f;
            }
        }
    }

    protected override void OnChargeComplete(Player player)
    {


        // Notify the boss controller about the completed charge
        if (bossController != null)
        {
            bossController.TentacleShieldChargeCompleted();
        }
    }

    public override void SetChargeIndicatorImage(Image image)
    {
        base.SetChargeIndicatorImage(image);
        if (chargeIndicatorImage != null)
        {
            chargeIndicatorImage.color = Color.green; // trying to make it green since its different from the other fields  
        }
    }

}