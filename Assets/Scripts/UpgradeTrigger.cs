using UnityEngine;

// Triggers the in-game upgrade menu
public class UpgradeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject upgradeMenu;

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
