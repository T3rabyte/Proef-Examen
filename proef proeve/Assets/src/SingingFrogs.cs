using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SingingFrogs : MonoBehaviour
{
    public List<GameObject> frogs;
    public List<GameObject> singingOrder;
    public List<GameObject> userOrder;
    public TMP_Text infoTekst;
    public int maxFrogs;

    public Sprite bZing;
    public Sprite bSit;
    public Sprite gZing;
    public Sprite gSit;
    public Sprite rZing;
    public Sprite rSit;
    public bool started = false;
    public int lastTap = 0;
    public GameObject redo;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SetFrogsEnabled(false);
        GenerateOrder();
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > lastTap && !started) 
        {
            started = true;
            StartSinging();
        }
    }

    public void Redo() 
    {
        SetFrogsEnabled(false);
        singingOrder = new List<GameObject>();
        GenerateOrder();
        StartSinging();
    }

    private void GenerateOrder() 
    {
        int index = UnityEngine.Random.Range(0, frogs.Count);
        if (singingOrder.Count != 0)
        {
            while (frogs[index] == singingOrder[singingOrder.Count - 1])
                index = UnityEngine.Random.Range(0, frogs.Count);

            singingOrder.Add(frogs[index]);
        }
        else 
        {
            singingOrder.Add(frogs[index]);
        }
    }

    public void StartSinging()
    {
        redo.SetActive(false);
        userOrder= new List<GameObject>();
        StartCoroutine(Singing());
    }

    private IEnumerator Singing() 
    {
        
        yield return new WaitForSeconds(1);
        infoTekst.text = "Let op! De kikkers zingen.";
        yield return new WaitForSeconds(1);
        foreach (GameObject obj in singingOrder) 
        {
            Image image = obj.GetComponent<Image>();
            switch (obj.name)
            {
                case "Blue":
                    image.sprite = bZing; 
                    break;
                case "Green":
                    image.sprite = gZing;
                    break;
                case "Red":
                    image.sprite = rZing;
                    break;
            }
            yield return new WaitForSeconds(1.2f);
            switch (obj.name)
            {
                case "Blue":
                    image.sprite = bSit;
                    break;
                case "Green":
                    image.sprite = gSit;
                    break;
                case "Red":
                    image.sprite = rSit;
                    break;
            }
            yield return new WaitForSeconds(1.2f);
        }
        SetFrogsEnabled(true);
        infoTekst.text = "Jouw Beurt!";
        yield break;
    }

    public void AddToUserOrder() 
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        userOrder.Add(obj);
        if (userOrder.Count == singingOrder.Count)
        {
            SetFrogsEnabled(false);
            CheckUserOrder();
        }
    }

    private void SetFrogsEnabled(bool value)
    {
        foreach (GameObject obj in frogs) 
        {
            obj.GetComponent<Button>().enabled = value;
        }
    }

    private void CheckUserOrder() 
    {
        bool wrongOrder = false;
        for (int i = 0; i < userOrder.Count; i++)
        {
            if (singingOrder[i].name != userOrder[i].name)
            {
                Debug.Log("Lost!");
                wrongOrder = true;
                infoTekst.text = "Fout!";
                StartCoroutine(FlashAllFrogs());
                userOrder = new List<GameObject>();
                CancelInvoke("CheckUserOrder");
                StartCoroutine(Singing());
                return;
            }
        }
        if (!wrongOrder)
        {
            Debug.Log("Win!");
            infoTekst.text = "Correct!";
            userOrder = new List<GameObject>();
            StartCoroutine(FlashAllFrogs());

            CancelInvoke("CheckUserOrder");
            if (singingOrder.Count < maxFrogs)
            {
                GenerateOrder();
                StartCoroutine(Singing());
            }
            else
            {
                infoTekst.text = "Gewonnen!";
                redo.SetActive(true);
            }
            return;
        }
    }

    private IEnumerator FlashAllFrogs() 
    {
        foreach (GameObject obj in frogs)
        {
            Image image = obj.GetComponent<Image>();
            switch (obj.name)
            {
                case "Blue":
                    image.sprite = bZing;
                    break;
                case "Green":
                    image.sprite = gZing;
                    break;
                case "Red":
                    image.sprite = rZing;
                    break;
            }
            
        }
        yield return new WaitForSeconds(1f);
        foreach (GameObject obj in frogs)
        {
            Image image = obj.GetComponent<Image>();
            switch (obj.name)
            {
                case "Blue":
                    image.sprite = bSit;
                    break;
                case "Green":
                    image.sprite = gSit;
                    break;
                case "Red":
                    image.sprite = rSit;
                    break;
            }
        }
        yield break;
    }
}
