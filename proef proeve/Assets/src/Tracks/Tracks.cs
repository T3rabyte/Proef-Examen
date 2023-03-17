using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tracks : Minigame
{
    public List<GameObject> animals;
    public GameObject uiPanel;
    public int animalsFound = 0;

    public override void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        allInputButtons = animals;
        StartMinigame();
    }

    public override void GenerateOrder()
    {

    }

    public override void StartMinigame()
    {
        userInputCap = 2;
    }

    public void SelectAnimal() 
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        obj.GetComponent<Image>().color = Color.green;
        obj.GetComponent<Button>().enabled = false;
        AddToUserOrder(obj);
    }
    
    public void SelectTrack()
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        obj.GetComponent<Image>().color = Color.green;
        obj.GetComponent<Button>().enabled = false;
        AddToUserOrder(obj);
    }

    public override void CheckUserOrder()
    {
        if (userInput[0].tag == userInput[1].tag)
        {
            animalsFound += 1;
            foreach (GameObject animal in userInput)
            {
                animal.SetActive(false);
                animal.GetComponent<Image>().color = Color.white;

            }
            userInput = new List<GameObject>();
            if (animalsFound == 3) 
            {
                uiPanel.SetActive(true);
            }
        }
        else 
        {
            foreach (GameObject animal in userInput) 
            {
                animal.GetComponent<Image>().color = Color.white;
            }
            userInput = new List<GameObject>();
        }
        SetButtonsEnabled(true);
    }

    public void Redo() 
    {
        foreach (GameObject animal in animals) 
        {
            animal.SetActive(true);
        }
        SetButtonsEnabled(true);
        uiPanel.SetActive(false);
    }

    public void back() 
    {
        SceneManager.LoadScene("Map");
    }
}
