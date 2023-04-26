using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RelayText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textInput;
    [SerializeField] private TextMeshProUGUI textOutput;

    // Update is called once per frame
    void Update()
    {
        textOutput.text = textInput.text;
    }
}
