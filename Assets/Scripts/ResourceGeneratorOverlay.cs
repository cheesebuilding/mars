using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceGeneratorOverlay : MonoBehaviour
{
    [SerializeField] private ResourceGenerator resourceGenerator;
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject text;

    private Transform barTransform;

    void Start()
    {
        ResourceGeneratorData resourceGeneratorData = resourceGenerator.GetResourceGeneratorData();

        barTransform = transform.Find("bar");

        icon.GetComponent<SpriteRenderer>().sprite = resourceGeneratorData.resourceType.sprite;
        
        text.GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
        
    }


    void Update()
    {
         barTransform.localScale = new Vector3(1 - resourceGenerator.GetTimerNormalized(), 1, 1);
         text.GetComponent<TextMeshPro>().SetText(resourceGenerator.GetAmountGeneratedPerSecond().ToString("F1"));
        
    }
}
