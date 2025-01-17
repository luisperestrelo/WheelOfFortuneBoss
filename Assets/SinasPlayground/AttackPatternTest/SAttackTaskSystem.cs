using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SAttackTaskSystem : MonoBehaviour
{
    public SAttackPattern attackPattern;

    void Awake()
    {
        attackPattern.Initialize(this);
    }

    public GameObject InstantiateAttack(GameObject  attackPrefab, float degree) {
        return Instantiate(attackPrefab, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, degree), transform);
    }

    async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            await attackPattern.StartAttack();
        } 
    }
}

