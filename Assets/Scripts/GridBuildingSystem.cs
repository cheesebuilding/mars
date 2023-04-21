using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.EventSystems;

public class GridBuildingSystem : MonoBehaviour {

    public static GridBuildingSystem Instance { get; private set; }

    public static bool selectUnit = true;

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;


    private Grid<GridObject> grid;
    private PlacedObjectTypeSO.Dir dir;

    private BuildingTypeListSO buildingTypeList;
    private PlacedObjectTypeSO activePlacedObjectType;

    private void Start(){
        buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypeList");
        activePlacedObjectType = null;

    }

    private void Awake() {
        Instance = this;

        int gridWidth = 30;
        int gridHeight = 30;
        float cellSize = 10f;
        grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

        activePlacedObjectType = null; 
    }

    public class GridObject {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject_Done placedObject;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject_Done placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject_Done GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {
            return placedObject == null;
        }

    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {

            if(activePlacedObjectType != null && ResourceManager.Instance.CanAfford(activePlacedObjectType.constructionResourceCostArray)){
                 ResourceManager.Instance.SpendResources(activePlacedObjectType.constructionResourceCostArray);
                    selectUnit = false;
                    Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
                    grid.GetXZ(mousePosition, out int x, out int z);

                    Vector2Int placedObjectOrigin = new Vector2Int(x, z);
                    placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

                    // Test Can Build
                    List<Vector2Int> gridPositionList = activePlacedObjectType.GetGridPositionList(placedObjectOrigin, dir);
                    bool canBuild = true;
                    foreach (Vector2Int gridPosition in gridPositionList) {
                        if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild() && !EventSystem.current.IsPointerOverGameObject()) {
                            selectUnit = false;
                            canBuild = false;
                            break;
                        }
                    }

                    if (canBuild) {
                        Vector2Int rotationOffset = activePlacedObjectType.GetRotationOffset(dir);
                        Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                        PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, activePlacedObjectType);

                        foreach (Vector2Int gridPosition in gridPositionList) {
                            grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                        }

                        OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                        UtilsClass.CreateWorldTextPopup(canBuild.ToString(), mousePosition);

                        //DeselectObjectType();
                    } 
                    else {
                        // Cannot build here
                        UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePosition);

                    }

               
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlacedObjectTypeSO.GetNextDir(dir);

            }
        if(activePlacedObjectType == null && !EventSystem.current.IsPointerOverGameObject() ){
            selectUnit = true;
        }

        
            
        }





        

        if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { activePlacedObjectType = buildingTypeList.list[0]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { activePlacedObjectType = buildingTypeList.list[1]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { activePlacedObjectType = buildingTypeList.list[2]; RefreshSelectedObjectType(); }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { activePlacedObjectType = buildingTypeList.list[3]; RefreshSelectedObjectType(); }
        
        
        



    }

    public void DeselectObjectType() {
        activePlacedObjectType = null; RefreshSelectedObjectType();
    }


    private void CheckUnit(){
        if(activePlacedObjectType = null){
            selectUnit = false;
        }
        else if(activePlacedObjectType != null){
            selectUnit = true;
        }
    }

    public void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (activePlacedObjectType != null) {
            Vector2Int rotationOffset = activePlacedObjectType.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (activePlacedObjectType != null) {
            return Quaternion.Euler(0, activePlacedObjectType.GetRotationAngle(dir), 0);
        } else {
            return Quaternion.identity;
        }
    }

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return activePlacedObjectType;
    }

    public void SetActiveBuildingType(PlacedObjectTypeSO placedObjectType){
        placedObjectType = activePlacedObjectType;
    }

    public PlacedObjectTypeSO GetActiveBuildingType(){
        return activePlacedObjectType;
    }

}
