using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlailAttack : MonoBehaviour
{
    [SerializeField] private GameObject flail;
    [SerializeField] private GameObject telegraph;
    [SerializeField] private Transform flailSpawnPoint;
    [SerializeField] private float telegraphDuration;
    
    [SerializeField] private float telegraphSequenceDelay = 1f;
    [SerializeField] private float slamSequenceDelay = 1f;

    [SerializeField] private float damage;
    private bool firstSlam = true;

    private GameObject activeFlail;

    public void Initialize(float damage)
    {
        this.damage = damage;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnFlailTelegraph(90f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnFlailTelegraph(180f);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnFlailTelegraph(0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnFlail(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SlamAtAngle(45f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SlamAtAngle(135f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SlamAtAngle(-45f);
        }   

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            TripleSlamSequence();
        }
#endif
    }

    public void SpawnFlailTelegraph(float rotation)
    {
        GameObject telegraphInstance = Instantiate(telegraph, flailSpawnPoint.position, Quaternion.Euler(0f, 0f, rotation));
        Destroy(telegraphInstance, telegraphDuration);
    }

    //TODO
    public void SpawnFlail(float rotation)
    {
        activeFlail = Instantiate(flail, flailSpawnPoint.position, Quaternion.Euler(0f, 0f, 270f));
        activeFlail.GetComponent<Animator>().enabled = false;
    }

    //voodoo to make animation work
    public void SlamAtAngle(float angle)
    {
        RotateFlail(angle);
        activeFlail.GetComponent<Animator>().enabled = true;
        if (firstSlam)
        {
            firstSlam = false;
        }
        else
        {
            activeFlail.GetComponent<Animator>().Play("Attack");
        }
    }

    // this is so ugly :D ideally we would have two separate Animations, one for "pulling back" and one for "slamming"
    public void RotateFlail(float angle)
    {
        StartCoroutine(RotateFlailDelayed(angle));
    }

    private IEnumerator RotateFlailDelayed(float angle)
    {
        yield return new WaitForSeconds(0.5f); 
        activeFlail.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void TripleSlamSequence()
    {
        StartCoroutine(TripleTelegraphSequenceCoroutine());
        StartCoroutine(TripleSlamSequenceCoroutine());
    }

    private IEnumerator TripleTelegraphSequenceCoroutine()
    {
        SpawnFlailTelegraph(90f);
        yield return new WaitForSeconds(telegraphSequenceDelay);

        SpawnFlailTelegraph(180f);
        yield return new WaitForSeconds(telegraphSequenceDelay);

        SpawnFlailTelegraph(0f);
    }

    private IEnumerator TripleSlamSequenceCoroutine()
    {
        SlamAtAngle(45f);
        yield return new WaitForSeconds(slamSequenceDelay);

        SlamAtAngle(135f);
        yield return new WaitForSeconds(slamSequenceDelay);

        SlamAtAngle(-45f);
    }
}
