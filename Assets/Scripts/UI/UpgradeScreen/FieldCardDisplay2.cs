using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FieldCardDisplay2 : UpgradeDisplayVisual
{
    [SerializeField] private List<RotateImage> selectObjects;
    // Start is called before the first frame update
 

    public override void Select()
    {
  
        selectObjects.ForEach(x => x.Play());   
 
    }
    
    public override void Deselect()
    {

        selectObjects.ForEach(x => x.Stop());   

    }
}