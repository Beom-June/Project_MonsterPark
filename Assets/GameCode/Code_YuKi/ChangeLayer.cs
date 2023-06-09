using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
   [SerializeField] private int customerLayer;
   [SerializeField] private int ignoreLayer;

   private void Start() 
   {
        customerLayer = LayerMask.NameToLayer("Customer");
        ignoreLayer = LayerMask.NameToLayer("IgnoreCamera");
   }
   private void OnTriggerEnter(Collider other) 
   {
        if (customerLayer == other.gameObject.layer)
        {
            ChangeLayerRecursively(other.gameObject, ignoreLayer);

        }
        else if (ignoreLayer == other.gameObject.layer)
        {
            ChangeLayerRecursively(other.gameObject, customerLayer);
        }
   }

    private void ChangeLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            ChangeLayerRecursively(child.gameObject, layer);
        }
    }
}
