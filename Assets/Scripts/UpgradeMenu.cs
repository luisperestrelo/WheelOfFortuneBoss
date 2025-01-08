using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] private Button chooseUpgradeButton;

    private void Start()
    {
        chooseUpgradeButton.onClick.AddListener(OnChooseUpgradeClicked);
    }

    public void OnChooseUpgradeClicked()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
} 