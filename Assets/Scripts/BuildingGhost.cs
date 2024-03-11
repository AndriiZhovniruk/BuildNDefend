using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spriteGameObject;
    private ResourceNearblyOverlay resourceNearblyOverlay;
    private void Awake()
    {
        spriteGameObject = transform.Find("sprite").gameObject;
        resourceNearblyOverlay = transform.Find("pfResourceNearblyOverlay").GetComponent<ResourceNearblyOverlay>();

        Hide();
    }

    private void Start()
    {
        BuildingManeger.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }
    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManeger.OnActiveBuildingTypeChangedEventArgs e)
    {
        if (e.activeBuildingType == null)
        {
            Hide();
            resourceNearblyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            if(e.activeBuildingType.hasResourceGeneratorData)
            {
                resourceNearblyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            }
            else
            {
                resourceNearblyOverlay.Hide();
            }
        }
    }

    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }
    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }
    private void Hide()
    {
        spriteGameObject.SetActive(false);
    }
}
