using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(48, 48);

    private SetBuildingMode _buildingManager;

    private Building[,] grid;
    [SerializeField] private Building flyingBuilding;
    private Camera mainCamera;

    // to place from other code
    private bool _isAvailableToPlace;
    private int _xPosition;
    private int _yPosition;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];

        mainCamera = Camera.main;
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        ResetBuilding();
        flyingBuilding = Instantiate(buildingPrefab);

        var groundPlane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

        if (groundPlane.Raycast(ray, out float position))
        {
            Vector3 worldPosition = ray.GetPoint(position);

            _xPosition = (int)(worldPosition.x / 3) * 3;
            _yPosition = (int)(worldPosition.z / 3) * 3;

            _isAvailableToPlace = true;

            if (_xPosition < 0 || _xPosition > GridSize.x - flyingBuilding.Size.x) _isAvailableToPlace = false;
            if (_yPosition < 0 || _yPosition > GridSize.y - flyingBuilding.Size.y) _isAvailableToPlace = false;

            flyingBuilding.transform.position = new Vector3(_xPosition, 0, _yPosition);

            if (_isAvailableToPlace && IsPlaceTaken()) _isAvailableToPlace = false;
            flyingBuilding.SetTransparent(_isAvailableToPlace);
        }
    }

    private void Update()
    {
        Debug.Log("azazazaza");

        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);;

                if (Input.GetMouseButtonDown(0))
                {
                    if (flyingBuilding.Type != BuildingType.Wall)
                    {
                        _xPosition = (int)(worldPosition.x / 3) * 3;
                        _yPosition = (int)(worldPosition.z / 3) * 3;

                        flyingBuilding.transform.position = new Vector3(_xPosition, flyingBuilding.transform.position.y, _yPosition);

                        _isAvailableToPlace = true;

                        if (_xPosition < 0 || _xPosition > GridSize.x - flyingBuilding.Size.x) _isAvailableToPlace = false;
                        if (_yPosition < 0 || _yPosition > GridSize.y - flyingBuilding.Size.y) _isAvailableToPlace = false;
                    }
                    else
                    {
                        Ray ray1 = mainCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray1, out hit))
                        {
                            if (hit.transform.tag == "WallPoint")
                            {
                                flyingBuilding.transform.position = hit.transform.position;
                                _isAvailableToPlace = true;
                            }
                        }
                    }

                    if (_isAvailableToPlace) _isAvailableToPlace = IsPlaceTaken();

                    flyingBuilding.SetTransparent(_isAvailableToPlace);
                }
            }
        }
    }

    private bool IsPlaceTaken()
    {
        if (flyingBuilding.Colliders.Count != 0)
            return false;

        return true;

        /*for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[_xPosition + x, _yPosition + y] != null) return true;
            }
        }

        return false;*/
    }

    public bool PlaceFlyingBuilding()
    {
        if (_isAvailableToPlace)
        {
           /* for (int x = 0; x < flyingBuilding.Size.x; x++)
            {
                for (int y = 0; y < flyingBuilding.Size.y; y++)
                {
                    grid[_xPosition + x, _yPosition + y] = flyingBuilding;
                }
            }
            */
            flyingBuilding.SetNormal();
            flyingBuilding = null;

            return true;
        }

        return false;
    }

    public void ResetBuilding()
    {
        if (flyingBuilding != null)
            Destroy(flyingBuilding.gameObject);

        flyingBuilding = null;
    }
}