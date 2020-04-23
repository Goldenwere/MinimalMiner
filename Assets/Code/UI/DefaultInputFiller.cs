using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefaultInputFiller : MonoBehaviour
{
    [SerializeField] private TMP_InputField field;
    [SerializeField] private string defaultValue;

    private void Awake()
    {
        field.SetTextWithoutNotify(defaultValue);
    }
}
