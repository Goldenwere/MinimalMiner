using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefaultSliderFiller : MonoBehaviour
{
    [SerializeField] private Slider sliderField;
    [SerializeField] private TextMeshProUGUI sliderText;
    [SerializeField] private float defaultValue;

    private void Awake()
    {
        sliderField.SetValueWithoutNotify(defaultValue);
        sliderText.text = defaultValue.ToString();
    }
}
