using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetParser : MonoBehaviour
{
    private Inventory _inventory;
    [SerializeField] private List<Target> _nearObjects;
    [SerializeField] private List<Resource> _nearResources;
    [SerializeField] private List<Target> _nearEnemies;
    [SerializeField] private Target _nearestTarget;
    [SerializeField] private Resource _nearestResource;

    private void Start()
    {
        _inventory = GetComponent<Inventory>();
    }

    public Target GetTarget()
    { 
        return _nearestTarget;
    }

    public Resource GetResourceTarget()
    { 
        return _nearestResource;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            _nearObjects.Add(other.GetComponent<Target>());

            if (other.GetComponent<Target>().Type == TargetType.Resource)
            {
                _nearResources.Add(other.GetComponent<Resource>());
            }
        }

        // add enemy parser
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Target")
        {
            if (_nearObjects.Count - 1 == 0)
            {
                _nearestTarget = null;
                _nearestResource = null;
            }

            _nearObjects.Remove(other.GetComponent<Target>());

            if (other.GetComponent<Target>().Type == TargetType.Resource)
            {
                _nearResources.Remove(other.GetComponent<Resource>());
            }
        }
    }

    private void FixedUpdate()
    {
        NearestResource();
        NearestTarget();
    }

    private void NearestResource()
    {
        if (_nearResources.Count != 0)
        {
            for (int i = 0; i < _nearResources.Count; i++)
            {
                if (_nearResources[i] != null)
                {
                    if (_nearestResource != null)
                    {
                        if (Vector3.Distance(transform.position, _nearestResource.transform.position) > Vector3.Distance(transform.position, _nearResources[i].transform.position))
                        {
                            if (_nearResources[i].RequiredTool != ToolType.None)
                            {
                                if (_inventory.FindTool(_nearResources[i].RequiredTool))
                                {
                                    _nearestResource = _nearResources[i];
                                }
                            }
                            else
                            {
                                _nearestResource = _nearResources[i];
                            }
                        }
                    }
                    else
                    {
                        if (_nearResources[i].RequiredTool != ToolType.None)
                        {
                            if (_inventory.FindTool(_nearResources[i].RequiredTool))
                            {
                                _nearestResource = _nearResources[i];
                            }
                        }
                        else
                        {
                            _nearestResource = _nearResources[i];
                        }
                    }
                }
                else
                {
                    _nearResources.Remove(_nearResources[i]);
                    return;
                }
            }
        }
    }

    private void NearestTarget()
    {
        if (_nearObjects.Count != 0)
        {
            for (int i = 0; i < _nearObjects.Count; i++)
            {
                if (_nearObjects[i] != null)
                {
                    if (_nearestTarget != null)
                    {
                        if (Vector3.Distance(transform.position, _nearestTarget.transform.position) > Vector3.Distance(transform.position, _nearObjects[i].transform.position))
                        {
                            _nearestTarget.transform.GetChild(0).gameObject.SetActive(false);
                            _nearestTarget = _nearObjects[i];
                        }

                        if (Vector3.Distance(transform.position, _nearestTarget.transform.position) <= _nearestTarget.CircleRenderDistance)
                        {
                            _nearestTarget.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else
                        {
                            _nearestTarget.transform.GetChild(0).gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        _nearestTarget = _nearObjects[i];
                    }
                }
                else
                {
                    _nearObjects.Remove(_nearObjects[i]);
                    return;
                }
            }
        }
    }
}
