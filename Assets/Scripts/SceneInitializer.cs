using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneInitializer : MonoBehaviour
{
    void Awake()
    {
        // Find the RunManager (which persisted from the previous scene)
        RunManager runManager = FindObjectOfType<RunManager>();

        // Find the WheelManager in the current scene
        WheelManager wheelManager = FindObjectOfType<WheelManager>();

        // Assign the WheelManager to the RunManager
        runManager.wheelManager = wheelManager;

        // Activate the WheelManager
//        wheelManager.gameObject.SetActive(true);

        // Re-initialize the wheel based on FieldCards in currentRunCards
        wheelManager.indicatorCanvas = GameObject.Find("IndicatorCanvas").GetComponent<Canvas>();
        runManager.wheelManager.InitializeWheel(runManager.currentRunCards.OfType<FieldCard>().Select(fc => fc.field).ToList());

        // Find the Player
        Player player = FindObjectOfType<Player>();

        // Activate the Player
  //      player.gameObject.SetActive(true);
    }
} 