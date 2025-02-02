using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSlowdown : MonoBehaviour
{
    [SerializeField] private GameObject attackFieldTutorial;

    private void Start()
    {
        Debug.Log("TutorialSlowdown enabled");
        Invoke(nameof(EnableAttackFieldTutorial), 2.9f);
    }

    private void EnableAttackFieldTutorial()
    {
        if (attackFieldTutorial != null)
        {
            attackFieldTutorial.SetActive(true);
            Destroy(attackFieldTutorial, 3f);
            Debug.Log("attackFieldTutorial enabled");
        }
    }
}
