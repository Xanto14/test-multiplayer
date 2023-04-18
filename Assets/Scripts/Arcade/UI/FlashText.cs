using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashText : MonoBehaviour
{
    public float flashSpeed = 0.5f; // The speed of the flashing effect
    public float minAlpha = 0.2f; // The minimum alpha value of the text
    public float maxAlpha = 1f; // The maximum alpha value of the text

    private TextMeshProUGUI textComponent; // Reference to the TextMeshProUGUI component
    private float currentAlpha; // The current alpha value of the text
    private bool isIncreasing = true; // Whether the alpha value is currently increasing or decreasing

    void Start()
    {
        // Get a reference to the TextMeshProUGUI component
        textComponent = GetComponent<TextMeshProUGUI>();

        // Set the current alpha value to the minimum value
        currentAlpha = minAlpha;
    }

    void Update()
    {
        // Gradually increase or decrease the alpha value
        if (isIncreasing)
        {
            currentAlpha += Time.deltaTime * flashSpeed;
            if (currentAlpha >= maxAlpha)
            {
                currentAlpha = maxAlpha;
                isIncreasing = false;
            }
        }
        else
        {
            currentAlpha -= Time.deltaTime * flashSpeed;
            if (currentAlpha <= minAlpha)
            {
                currentAlpha = minAlpha;
                isIncreasing = true;
            }
        }

        // Update the alpha value of the text component
        textComponent.color = new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, currentAlpha);
    }
}
