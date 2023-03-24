using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelection : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    private int currentShip;
    private void Awake()
    {
        SelectShip(0);
    }
    private void SelectShip (int index)
    {
        nextButton.interactable= (index<transform.childCount-1);
        previousButton.interactable = (index > 0);

        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i==index);
    }

    public void OnClick_ChangeShip(int choix)
    {
        currentShip += choix;
        SelectShip(currentShip);
    }
}
