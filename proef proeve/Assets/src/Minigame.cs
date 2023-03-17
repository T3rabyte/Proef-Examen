using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Minigame : MonoBehaviour
{
    public List<GameObject> userInput;
    public List<GameObject> gameInput;
    public List<GameObject> allInputButtons;

    public int userInputCap;

    public abstract void Start();

    public abstract void GenerateOrder();

    public abstract void StartMinigame();

    public abstract void CheckUserOrder();

    public void SetButtonsEnabled(bool value) 
    {
        foreach (GameObject button in allInputButtons) 
        {
            button.GetComponent<Button>().enabled = value;
        }
    }

    public void AddToUserOrder(GameObject obj = null)
    {
        if(obj == null)
            obj = EventSystem.current.currentSelectedGameObject;
        userInput.Add(obj);
        if (userInput.Count == userInputCap)
        {
            SetButtonsEnabled(false);
            CheckUserOrder();
        }
    }
}
