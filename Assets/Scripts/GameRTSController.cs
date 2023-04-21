
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GameRTSController : MonoBehaviour {

    [SerializeField] private Transform selectionAreaTransform = null;


    private Vector3 startPosition;
    private List<UnitRTS> selectedUnitRTSList;

    private void Awake() {
        selectedUnitRTSList = new List<UnitRTS>();
        selectionAreaTransform.gameObject.SetActive(false);
    }

    private void Update() {
        if(Input.GetMouseButtonDown(1)){
            
            Vector3 moveToPosition = Mouse3D.GetMouseWorldPosition();

            

            List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[]{1f, 2f, 3f}, new int []{5, 10, 20});
            int targetPositionListIndex = 0;
            foreach (UnitRTS unitRTS in selectedUnitRTSList){
                unitRTS.MoveTo(targetPositionList[targetPositionListIndex]);
                
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;

            }
        }
        if (Input.GetMouseButtonDown(0) && !UtilsClass.IsPointerOverUI()) {
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = Mouse3D.GetMouseWorldPosition();

            DeselectAllUnits();
        }

        if (Input.GetMouseButton(0)) {
            // Left Mouse Button Held Down
            CalculateSelectionLowerLeftUpperRight(out Vector3 lowerLeft, out Vector3 upperRight);
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }

        if (Input.GetMouseButtonUp(0)) {
            // Hide visual even if over the UI
            selectionAreaTransform.gameObject.SetActive(false);
        }

        if (Input.GetMouseButtonUp(0) && !UtilsClass.IsPointerOverUI()) {
            CalculateSelectionLowerLeftUpperRight(out Vector3 lowerLeft, out Vector3 upperRight);

            // Calculate Center and Extents
            Vector3 selectionCenterPosition = new Vector3(
                lowerLeft.x + ((upperRight.x - lowerLeft.x) / 2f),
                0,
                lowerLeft.z + ((upperRight.z - lowerLeft.z) / 2f)
            );

            Vector3 halfExtents = new Vector3(
                (upperRight.x - lowerLeft.x) * .5f,
                1,
                (upperRight.z - lowerLeft.z) * .5f
            );

            // Set min size
            float minSelectionSize = .5f;
            if (halfExtents.x < minSelectionSize) halfExtents.x = minSelectionSize;
            if (halfExtents.z < minSelectionSize) halfExtents.z = minSelectionSize;

            // Find Objects within Selection Area
            Collider[] colliderArray = Physics.OverlapBox(selectionCenterPosition, halfExtents);
            foreach (Collider collider in colliderArray) {

                if (collider.TryGetComponent<UnitRTS>(out UnitRTS unitRTS)) {

                        unitRTS.SetSelectedVisible(true);
                        selectedUnitRTSList.Add(unitRTS);
                }
            }
        }
        
    }
    

    private void CalculateSelectionLowerLeftUpperRight(out Vector3 lowerLeft, out Vector3 upperRight) {
        Vector3 currentMousePosition = Mouse3D.GetMouseWorldPosition();
        lowerLeft = new Vector3(
            Mathf.Min(startPosition.x, currentMousePosition.x),
            0,
            Mathf.Min(startPosition.z, currentMousePosition.z)
        );
        upperRight = new Vector3(
            Mathf.Max(startPosition.x, currentMousePosition.x),
            0,
            Mathf.Max(startPosition.z, currentMousePosition.z)
        );
    }

    private void DeselectAllUnits() {
        foreach (UnitRTS unitRTS in selectedUnitRTSList) {
            unitRTS.SetSelectedVisible(false);
        }

        selectedUnitRTSList.Clear();
    }

    public List<UnitRTS> GetSelectedUnitList() {
        return selectedUnitRTSList;
    }

    private List<Vector3> GetPositionListAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray){
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPosition);
        for (int i = 0; i< ringDistanceArray.Length; i++){
            positionList.AddRange(GetPositionListAround(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }


    private List<Vector3> GetPositionListAround(Vector3 startPosition, float distance, int positionCount){
        List<Vector3> positionList = new List<Vector3>();

        for (int i = 0; i< positionCount; i++){
            float angle = i * (360f/ positionCount) /100;
            Vector3 dir = ApplyRotationToVector(new Vector3(0, 10, 0), angle);
            Vector3 position = startPosition + dir * distance;
            positionList.Add(position);
        }
        return positionList;
    }
    private Vector3 ApplyRotationToVector(Vector3 vec, float angle){
        return Quaternion.Euler(0, 0, angle) * vec;
    }

}