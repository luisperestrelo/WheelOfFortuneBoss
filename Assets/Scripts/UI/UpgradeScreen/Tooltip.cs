using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform rect;

    public RectTransform Rect { get => rect; private set => rect = value; }
    public string Text
    {
        get => text.text;
        set
        {
            text.text = value;
        }
    }
}
