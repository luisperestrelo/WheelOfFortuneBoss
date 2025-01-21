using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFightOnStart : MonoBehaviour
{
    void Start()
    {
        RunManager.Instance.StartFight();
    }
}
