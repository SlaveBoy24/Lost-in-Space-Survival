using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("MainCharacter");
    }

    public void Auto()
    {
        _player.GetComponent<CharacterGathering>().SetState("AutoGathering");
    }

    public void Gather()
    {
        Target target;

        if (_player != null)
        {
            target = _player.GetComponent<TargetParser>().GetTarget();

            if (target != null && Vector3.Distance(_player.transform.position, target.transform.position) <= target.CircleRenderDistance)
            {
                if (target.Type == TargetType.Resource)
                {
                    _player.GetComponent<CharacterGathering>().SetState("Gathering", target.GetComponent<Resource>());
                }
                else if (target.Type == TargetType.Struct)
                {
                    target.GetComponent<StructInteract>().SetUse(_player.transform);
                }
            }
        }
        else
        {
            _player = GameObject.FindGameObjectWithTag("MainCharacter");
        }
    }
}
