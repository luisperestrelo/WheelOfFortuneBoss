using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PoisonCounter : MonoBehaviour
{
    private BuffManager buffManager;
    private TextMeshPro numberText;
    private SpriteRenderer iconImage;

    void Start()
    {
        buffManager = GetComponentInParent<BuffManager>();
        numberText = GetComponentInChildren<TextMeshPro>();
        iconImage = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        int poisonStacks = buffManager.GetTotalStacksOf("Poison");
        if (poisonStacks > 0)
        {
            numberText.text = poisonStacks.ToString();
            iconImage.enabled = true;
        }
        else
        {
            numberText.text = "";
            iconImage.enabled = false;
        }
    }
}
