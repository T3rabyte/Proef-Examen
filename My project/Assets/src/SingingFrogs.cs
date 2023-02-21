using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingingFrogs : MonoBehaviour
{
    public List<GameObject> frogs;
    public List<GameObject> singingOrder;
    public List<GameObject> userOrder;
    public int singingFrogs;

    // Start is called before the first frame update
    void Start()
    {
        SetFrogsEnabled(false);
        GenerateOrder();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateOrder() 
    {
        for (int i = 1; i < singingFrogs+1; i++) 
        {
            int index = Random.Range(0, frogs.Count);
            if (singingOrder.Count != 0)
                while (frogs[index] == singingOrder[singingOrder.Count-1])
                    index = Random.Range(0, frogs.Count);

            singingOrder.Add(frogs[index]);
        }
    }

    public void StartSinging()
    {
        userOrder= new List<GameObject>();
        StartCoroutine(Singing());
    }

    private IEnumerator Singing() 
    {
        
        yield return new WaitForSeconds(3);
        foreach (GameObject obj in singingOrder) 
        {
            Image image = obj.GetComponent<Image>();
            image.color = Color.green;
            yield return new WaitForSeconds(1.2f);
            image.color = Color.white;
            yield return new WaitForSeconds(1.2f);
        }
        SetFrogsEnabled(true);
        InvokeRepeating("CheckUserOrder", 0f, 1f);
        yield break;
    }

    public void AddToOrder() 
    {
        GameObject obj = EventSystem.current.currentSelectedGameObject;
        userOrder.Add(obj);
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
        if (singingOrder.Count == userOrder.Count) 
        {
            for (int i = 0; i < userOrder.Count; i++)
            {
                if (singingOrder[i].name != userOrder[i].name)
                {
                    Debug.Log("Lost!");
                    StartCoroutine(FlashAllForgs(Color.red));
                    userOrder = new List<GameObject>();
                    CancelInvoke("CheckUserOrder");
                    return;
                }
            }
            Debug.Log("Win!");
            userOrder = new List<GameObject>();
            StartCoroutine(FlashAllForgs(Color.green));
            SetFrogsEnabled(false);
            CancelInvoke("CheckUserOrder");
            return;
        }
        if (userOrder.Count > singingOrder.Count) 
        {
            Debug.Log("Lost!");
            StartCoroutine(FlashAllForgs(Color.red));
            userOrder = new List<GameObject>();
            CancelInvoke("CheckUserOrder");
            return;
        }
    }

    private IEnumerator FlashAllForgs(Color color) 
    {
        foreach (GameObject obj in frogs)
        {
            obj.GetComponent<Image>().color = color;
        }
        yield return new WaitForSeconds(1f);
        foreach (GameObject obj in frogs)
        {
            obj.GetComponent<Image>().color = Color.white;
        }
        yield break;
    }
}
