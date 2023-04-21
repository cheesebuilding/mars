using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    
    [SerializeField]private ResourceTypeSO resourceType;
    private float timer;
    private ResourceGeneratorData resourceGeneratorData;
    private int nearbyResourceAmount = 0;
    [SerializeField] private float timerMax;
    [SerializeField] private int generateResourceAmount = 1;
    [SerializeField] private int resourceDetectionAmount;
    [SerializeField] List<Transform> Units = new List<Transform>();
    [SerializeField] List<int> UnitsAmount = new List<int>();

    private void Awake(){
        resourceGeneratorData = GetComponent<ResourceTypeHolder>().resourceType.resourceGeneratorData;
      
        

    }

    private void Update(){




        
        
        

           timer -= Time.deltaTime;

    if (timer <= 0) {
        timer += timerMax;

        int totalUnitsAmount = 0;
        foreach (Transform unit in Units) {
            if (Vector3.Distance(transform.position, unit.position) < resourceDetectionAmount) {
                int unitAmount = UnitsAmount[Units.IndexOf(unit)];
                totalUnitsAmount += unitAmount;
            }
        }
        nearbyResourceAmount = totalUnitsAmount;

        ResourceManager.Instance.AddResource(resourceType, generateResourceAmount * nearbyResourceAmount);
    }
}
    public ResourceGeneratorData GetResourceGeneratorData(){
        return resourceGeneratorData;
    }

    public float GetTimerNormalized(){
        return timer/ timerMax;
    }

    public float GetAmountGeneratedPerSecond(){
        return generateResourceAmount * nearbyResourceAmount;
    }


        
        
    }

