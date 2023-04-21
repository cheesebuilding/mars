using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFacility : MonoBehaviour
{
    

    [SerializeField] private int unitDetectionAmount;
    [SerializeField] List<Transform> Units = new List<Transform>();
    [SerializeField] List<int> UnitsAmount = new List<int>();
    [SerializeField] private GameObject boxButton;

    private void Awake(){
        boxButton.SetActive(false);
    }


    private void Update(){




        
        
        




        int totalUnitsAmount = 0;
        foreach (Transform unit in Units) {
            if (Vector3.Distance(transform.position, unit.position) < unitDetectionAmount) {
                int unitAmount = UnitsAmount[Units.IndexOf(unit)];
                totalUnitsAmount += unitAmount;
                boxButton.SetActive(true);
            }
            else{
                boxButton.SetActive(false);

            }
        }


       
    
}



        
        
}

