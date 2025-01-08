using UnityEngine;

// Triggers the in-game upgrade menu
public class UpgradeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject UICanvas;

    private void Awake()
    {
        UICanvas = GameObject.Find("UICanvas");
        upgradeMenu = UICanvas.transform.Find("UpgradeMenu").gameObject;
    }
    
    private void Start()
    {
        //upgradeMenu = canvas.transform.Find("UpgradeMenu").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            upgradeMenu.SetActive(true);
            Destroy(gameObject);
            Time.timeScale = 0f;
        }
    }
}
