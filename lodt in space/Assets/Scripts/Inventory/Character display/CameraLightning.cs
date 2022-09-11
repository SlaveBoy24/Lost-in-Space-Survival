using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLightning : MonoBehaviour
{
    [SerializeField] private Light _ignoreLight;

    private void OnPreCull()
    {
        EnableLight(false);
    }

    private void OnPreRender()
    {
        EnableLight(false);
    }

    private void OnPostRender()
    {
        EnableLight(true);
    }

    private void EnableLight(bool value)
    {
        if (_ignoreLight != null)
        {
            _ignoreLight.enabled = value;
        }
    }
}
