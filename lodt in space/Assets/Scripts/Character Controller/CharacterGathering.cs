using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGathering : MonoBehaviour
{
    private Animator _animator;
    private string _state;
    private Resource _target;

    [Header("AutoGathering")]
    [SerializeField] private Transform _resourcesList;
    [SerializeField] private List<Resource> _toolRequiredResources;
    [SerializeField] private List<Resource> _resources;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _resourcesList = GameObject.FindGameObjectWithTag("ResourcesList").transform;

        for (int i = 0; i < _resourcesList.childCount; i++)
        {
            if (_resourcesList.GetChild(i).GetComponent<Resource>().RequiredTool == ToolType.None)
                _resources.Add(_resourcesList.GetChild(i).GetComponent<Resource>());
            else
                _toolRequiredResources.Add(_resourcesList.GetChild(i).GetComponent<Resource>());
        }
    }

    public void StopDoing()
    {
        StopAllCoroutines();
    }

    public void SetState(string state, Resource target = null)
    {
        StopDoing();

        _state = state;
        if (target != null)
            _target = target;

        if (_state == "Gathering")
            StartCoroutine(Gather());
        else if (_state == "AutoGathering")
            StartCoroutine(AutoGathering());
    }

    public IEnumerator AutoGathering()
    {
        if (_resourcesList.childCount == 0)
            StopDoing();

        while (_resources.Count != 0 || _toolRequiredResources.Count != 0)
        {
            Resource target = null;

            if (GetComponent<TargetParser>().GetResourceTarget() != null)
            {
                target = GetComponent<TargetParser>().GetResourceTarget();
            }
            
            if (target == null)
            {
                if (_resources.Count != 0)
                    target = _resources[0];
            }

            Vector3 vectorToTarget = target.transform.position - transform.position;
            while (Vector3.Distance(transform.position, target.transform.position) > target.GatherDistance)
            {
                Quaternion lookDir = Quaternion.LookRotation(vectorToTarget);
                Quaternion targetRot = Quaternion.Slerp(transform.rotation, lookDir, 0.025f);
                transform.rotation = targetRot;

                _animator.SetFloat("Moving", 1f);
                yield return null;
            }

            _animator.SetFloat("Moving", 0f);

            if (target.RequiredTool == ToolType.None)
            {
                _animator.SetTrigger("Looting");
                yield return new WaitForSeconds(0.75f);
                GetComponent<Inventory>().AddItem(target.Item, target.Amount);
                Destroy(target.gameObject);
/*                GetComponent<ResourceTargetParser>().RemoveTarget(target);*/
                _resources.Remove(target);

                GetComponent<Experience>().SetExperience(10);
            }
            else
            {
                if (GetComponent<Inventory>().FindTool(target.RequiredTool))
                {
                    GetComponent<Inventory>().UseTool(target.RequiredTool);

                    if (_target.RequiredTool == ToolType.Axe)
                        _animator.SetTrigger("AxeGathering");
                    else if (_target.RequiredTool == ToolType.Pickaxe)
                        _animator.SetTrigger("PickaxeGathering");

                    yield return new WaitForSeconds(0.8f);
                    target.Healths -= 1;

                    if (target.Healths <= 0)
                    {
                        GetComponent<Inventory>().AddItem(target.Item, target.Amount);
                        Destroy(target.gameObject);
                        /*                       GetComponent<ResourceTargetParser>().RemoveTarget(target); //destroy obj*/
                        _toolRequiredResources.Remove(target);

                        GetComponent<Experience>().SetExperience(50);
                    }

                    yield return new WaitForSeconds(0.8f);
                }
            }

            yield return new WaitForSeconds(0.75f);
        }

        _state = "";
    }

    public IEnumerator Gather()
    {
        Vector3 vectorToTarget = _target.transform.position - transform.position;
        while (Vector3.Distance(transform.position, _target.transform.position) > _target.GatherDistance)
        {
            transform.rotation = Quaternion.LookRotation(vectorToTarget * Time.deltaTime);
            _animator.SetFloat("Moving", 1f);
            yield return null;
        }

        _animator.SetFloat("Moving", 0f);

        if (_target.RequiredTool == ToolType.None)
        {
            _animator.SetTrigger("Looting");
            yield return new WaitForSeconds(0.75f);
            GetComponent<Inventory>().AddItem(_target.Item, _target.Amount);
            Destroy(_target.gameObject);
            /*            GetComponent<ResourceTargetParser>().RemoveTarget(_target); //destroy obj*/
            _resources.Remove(_target);

            GetComponent<Experience>().SetExperience(10);
        }
        else
        {
            if (GetComponent<Inventory>().FindTool(_target.RequiredTool))
            {
                GetComponent<Inventory>().UseTool(_target.RequiredTool);

                if (_target.RequiredTool == ToolType.Axe)
                    _animator.SetTrigger("AxeGathering");
                else if (_target.RequiredTool == ToolType.Pickaxe)
                    _animator.SetTrigger("PickaxeGathering");

                yield return new WaitForSeconds(0.75f);
                _target.Healths -= 1;

                if (_target.Healths <= 0)
                {
                    GetComponent<Inventory>().AddItem(_target.Item, _target.Amount);
                    Destroy(_target.gameObject);
                    /*                    GetComponent<ResourceTargetParser>().RemoveTarget(_target); //destroy obj*/
                    _toolRequiredResources.Remove(_target);

                    GetComponent<Experience>().SetExperience(50);
                }
            }
        }

        _state = "";
    }
}
