using UnityEngine;

public class Rune : MonoBehaviour
{
    [SerializeField] private RadialProgressBar chargeProgressBar;
    [SerializeField] private Color color; // not used yet
    [SerializeField] private Sprite icon;
    [SerializeField] private RuneProgress inactiveProgress;
    [SerializeField] private RuneProgress activeProgress;
    // lazy
    [SerializeField] private SpriteRenderer cooldownIcon;
    [SerializeField] private SpriteRenderer inactiveIcon;
    [SerializeField] private SpriteRenderer activeIcon;

    public void Initialize(Sprite icon)
    {
        cooldownIcon.sprite = icon;
        inactiveIcon.sprite = icon;
        activeIcon.sprite = icon;
        SetCooldownProgress(1f);
    }


    public void SetChargeProgress(float value)
    {
        chargeProgressBar.SetProgress(value);
    }

    /// <summary>
    /// 0 -> Cooldown starts <br/>
    /// 1 -> Cooldown completed
    /// </summary>
    /// <param name="value"></param>
    public void SetCooldownProgress(float value)
    {
        inactiveProgress.SetProgress(value);
    }
    


    // TODO
    public void SetActiveProgress(float value)
    {
        activeProgress.SetProgress(value);
    }


}