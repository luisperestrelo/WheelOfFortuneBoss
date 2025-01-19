using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
public class AutoLayerSort : MonoBehaviour
{

    private SortingGroup sortingGroup;
    private const float BOSS_THRESHOLD_X = 4.78f;

    private void Awake()
    {
        sortingGroup = GetComponent<SortingGroup>();
    }

    public void SetSortingLayer()
    {
        if (transform.position.y <= BOSS_THRESHOLD_X) 
        {
            sortingGroup.sortingLayerName = "frontOfBoss";
        }
    }
}
