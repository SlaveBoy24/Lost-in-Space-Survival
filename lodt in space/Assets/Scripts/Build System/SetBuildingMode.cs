using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBuildingMode : MonoBehaviour
{
    [SerializeField] private BuildingsGrid _buildingGrid;
    [SerializeField] private GameObject _interactButtons;

    public void SelectBuilding(Building selectedStructPrefab)
    {
        if (_buildingGrid != null)
        { 
            _buildingGrid.StartPlacingBuilding(selectedStructPrefab);
        }
    }

    private void OnEnable()
    {
        if (_buildingGrid == null)
        {
            _buildingGrid = GameObject.FindGameObjectWithTag("HomeTerritory").GetComponent<BuildingsGrid>();
        }

        if (_buildingGrid == null)
            this.gameObject.SetActive(false);

        Camera.main.GetComponent<CameraFollow>().SetState("Building");
    }

    private void OnDisable()
    {
        if (_buildingGrid != null)
            _buildingGrid.ResetBuilding();

        Camera.main.GetComponent<CameraFollow>().SetState("");
    }

    private void DenyPlacing()
    {
        if (_buildingGrid != null)
            _buildingGrid.ResetBuilding();

        _interactButtons.SetActive(false);
    }

    private void ApplyPlacing()
    {
        if (_buildingGrid != null)
        {
            if (_buildingGrid.PlaceFlyingBuilding())
                _interactButtons.gameObject.SetActive(false);
        }

    }

    private void HideInteractButtons()
    { 
        _interactButtons.SetActive(false);
    }

    private void ShowInteractButtons()
    {
        _interactButtons.SetActive(true);
    }
}
