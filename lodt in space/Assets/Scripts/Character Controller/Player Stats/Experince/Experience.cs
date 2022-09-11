using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Experience : MonoBehaviour
{
    [SerializeField] private LevelDisplay _displayingScript;
    [SerializeField] private int _experinceToLevelUp;
    [SerializeField] private int _experience;
    [SerializeField] private int _level;
    [SerializeField] private Slider _experinceSlider; // полоса опыта
    [SerializeField] private TextMeshProUGUI _levelText; // отображение опыта

    private void Start()
    {
        UpdateDisplayingInfo();
    }

    public void SetExperience(int value)
    {
        _experience += value;

        int less = _experience - _experinceToLevelUp;
        if (less >= 0)
        {
            _level++;
            _experience = less;
            RecalculateExperience();
            DisplayLevelUp();
        }

        UpdateDisplayingInfo();
    }

    private void UpdateDisplayingInfo()
    {
        _experinceSlider.maxValue = _experinceToLevelUp;
        _experinceSlider.value = _experience;
        _levelText.text = "" + _level;
    }

    private void DisplayLevelUp()
    {
        _displayingScript.StartDisplaying(_level);
    }

    private void RecalculateExperience()
    {
        _experinceToLevelUp = Convert.ToInt32(_experinceToLevelUp * 1.2f);
    }
}
