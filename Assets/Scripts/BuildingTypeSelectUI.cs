using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class BuildingTypeSelectUI : MonoBehaviour {

    [SerializeField] private Sprite arrowSprite;

    private Dictionary<PlacedObjectTypeSO, Transform> btnTransformDictionary;
    private Transform arrowBtn;
    private BuildingTypeListSO buildingTypeList;
    private PlacedObjectTypeSO activePlacedObjectType;

    private void Awake() {
        Transform btnTemplate = transform.Find("btnTemplate");
        Transform textTemplate = transform.Find("text");
        btnTemplate.gameObject.SetActive(false);

        buildingTypeList = Resources.Load<BuildingTypeListSO>("BuildingTypeList");

        btnTransformDictionary = new Dictionary<PlacedObjectTypeSO, Transform>();

        int index = 0;

        arrowBtn = Instantiate(btnTemplate, transform);
        arrowBtn.gameObject.SetActive(true);

        float offsetAmount = +130f;
        arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
        arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -40);

        arrowBtn.GetComponent<Button>().onClick.AddListener(() => {
            GridBuildingSystem.Instance.SetActiveBuildingType(null);
            GridBuildingSystem.Instance.DeselectObjectType();
        });

        index++;

        


        foreach (PlacedObjectTypeSO placedObjectType in buildingTypeList.list) {
            Transform btnTransform = Instantiate(btnTemplate, transform);
            btnTransform.gameObject.SetActive(true);

            offsetAmount = +130f;
            btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            btnTransform.Find("image").GetComponent<Image>().sprite = placedObjectType.sprite;
            

            btnTransform.GetComponent<Button>().onClick.AddListener(() => {
                Debug.Log(placedObjectType);
                GridBuildingSystem.Instance.RefreshSelectedObjectType();
                GridBuildingSystem.Instance.SetActiveBuildingType(placedObjectType);
                
                UpdateActiveBuildingTypeButton();
                
            });

            btnTransformDictionary[placedObjectType] =  btnTransform;
            Debug.Log(placedObjectType);

            index++;
        }
    }

    private void Update() {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton() {
    arrowBtn.Find("selected").gameObject.SetActive(false);
    foreach (PlacedObjectTypeSO placedObjectType in btnTransformDictionary.Keys) {
        Transform btnTransform = btnTransformDictionary[placedObjectType];
        btnTransform.Find("selected").gameObject.SetActive(false);
    }

    PlacedObjectTypeSO activePlacedObjectType= GridBuildingSystem.Instance.GetActiveBuildingType();
    if (activePlacedObjectType == null) {
        arrowBtn.Find("selected").gameObject.SetActive(true);
    } else if (btnTransformDictionary.ContainsKey(activePlacedObjectType)) {
        btnTransformDictionary[activePlacedObjectType].Find("selected").gameObject.SetActive(true);
    }
}

}
