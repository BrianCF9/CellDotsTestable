using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;


public class ValueControl : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Button increaseButton; // Arrastra el botón "Increase" en el Inspector
    [SerializeField] private Button decreaseButton; // Arrastra el botón "Decrease" en el Inspector

    [Header("Events")]
    private Action<int> _onValueChanged;


    #region Properties
    public Action<int> OnValueChanged { get => _onValueChanged; set => _onValueChanged = value; }
    public int Value => currentValue;
    #endregion

    private int minValue = 30;
    private int maxValue = 60;
    private int step = 1;
    private int currentValue = 60;

    public void Configure(int minValue, int maxValue, int step, int initialValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.step = step;
        this.currentValue = initialValue;

        // Asegúrate de que el valor inicial esté dentro del rango
        this.currentValue = Mathf.Clamp(this.currentValue, this.minValue, this.maxValue);
        UpdateValueText();
    }

    void OnEnable()
    {

        increaseButton.onClick.AddListener(IncreaseValue);
        decreaseButton.onClick.AddListener(DecreaseValue);
    }
    void OnDisable()
    {
        increaseButton.onClick.RemoveListener(IncreaseValue);
        decreaseButton.onClick.RemoveListener(DecreaseValue);
    }

    public void IncreaseValue()
    {
        currentValue = Mathf.Clamp(currentValue + step, minValue, maxValue);
        UpdateValueText();
    }

    public void DecreaseValue()
    {
        currentValue = Mathf.Clamp(currentValue - step, minValue, maxValue);
        UpdateValueText();
    }

    private void UpdateValueText()
    {
        valueText.text = currentValue.ToString();
        _onValueChanged?.Invoke(currentValue);
    }
}
