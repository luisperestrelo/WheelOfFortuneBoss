using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneInitializer : MonoBehaviour
{
    void Awake()
    {
        RunManager runManager = FindObjectOfType<RunManager>();

        WheelManager wheelManager = FindObjectOfType<WheelManager>();

        runManager.wheelManager = wheelManager;


        // Re-initialize the wheel based on FieldCards in currentRunCards
        wheelManager.indicatorCanvas = GameObject.Find("IndicatorCanvas").GetComponent<Canvas>();
        runManager.wheelManager.InitializeWheel(runManager.currentRunCards.OfType<FieldCard>().Select(fc => fc.field).ToList());

        Player player = FindObjectOfType<Player>();


        MidFightCardOfferUI midFightCardOfferUI = FindObjectOfType<MidFightCardOfferUI>();

        runManager.midFightCardOfferUI = midFightCardOfferUI;
    }
} 