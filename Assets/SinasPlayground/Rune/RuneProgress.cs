using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RuneProgress : MonoBehaviour
{

    [SerializeField] private Transform _mask;
    [Range(0, 1)] public float progress = 0f; 


     private float _progress = 0f;


    private Vector3 _scale;


    public float Progress
    {
        get { return _progress; }
        set
        {
            _progress = value;
            _mask.localScale = new Vector3(_scale.x, value * _scale.y, 1);
        }
    }

    void Awake(){
        _scale = _mask.localScale;

    }

    void Update() {
        _mask.localScale = new Vector3(_scale.x, progress * _scale.y, 1);
    }


}
