using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelText;
    public void StartDisplaying(int level)
    {
        _levelText.text = level.ToString();
        gameObject.SetActive(true);
        StartCoroutine("Displaylevel");
    }

    private IEnumerator Displaylevel()
    {
        yield return new WaitForSeconds(2);

        gameObject.SetActive(false);
    }
}
