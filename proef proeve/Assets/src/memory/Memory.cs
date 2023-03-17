using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Memory : Minigame
{
    public GameObject panel;
    public List<GameObject> cards;
    public Sprite cardBack;
    public GameObject redo;

    public int cardsFound = 0;

    public override void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        GenerateOrder();
        StartMinigame();
    }

    // Start is called before the first frame update
    public override void StartMinigame()
    {
        userInputCap = 2;
        InstantiateCards();
    }

    public override void GenerateOrder()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject card = cards[UnityEngine.Random.Range(0, cards.Count)];
            if (gameInput.Count != 0)
            {
                int timesFound = 0;
                foreach (GameObject obj in gameInput)
                {
                    if (obj.transform.name == card.transform.name)
                    {
                        timesFound++;
                        if (timesFound == 2)
                        {
                            cards.Remove(card);
                            i--;
                        }
                    }
                }
                if (timesFound < 2)
                {
                    if (gameInput[gameInput.Count - 1].transform.name == card.transform.name)
                    {
                        i--;
                    }
                    else
                    {
                        gameInput.Add(card);
                    }
                }
            }
            else
            {
                gameInput.Add(card);
            }
        }
    }

    private void InstantiateCards() 
    {
        foreach (GameObject card in gameInput)
        {
            GameObject instCard = Instantiate(card, transform.position, Quaternion.identity);
            instCard.transform.SetParent(panel.transform);
            instCard.GetComponent<Button>().onClick.AddListener(delegate { StartCoroutine(TurnOverCard(instCard)); });
            allInputButtons.Add(instCard);
        }
    }

    public override void CheckUserOrder()
    {
        if (userInput[0].transform.name == userInput[1].transform.name) 
        {
            cardsFound += 1;
            foreach (GameObject card in userInput) 
            {
                card.GetComponent<Button>().enabled = false;
            }
            userInput = new List<GameObject>();
            SetButtonsEnabled(true);
            if (cardsFound == 5) 
            {
                redo.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(ResetCards());
        }
    }

    private IEnumerator TurnOverCard(GameObject instCard) 
    {
        Animator cardAnim = instCard.GetComponent<Animator>();
        cardAnim.enabled = true;
        cardAnim.Play("Open");
        yield return new WaitForSeconds(0.4f);
        cardAnim.enabled = false;
        instCard.GetComponent<Image>().sprite = instCard.GetComponent<Card>().front;
        AddToUserOrder(instCard);
        instCard.GetComponent<Button>().enabled = false;
        yield break;
    }

    private IEnumerator ResetCards() 
    {
        yield return new WaitForSeconds(1f);
        foreach (GameObject card in userInput) 
        {
            card.GetComponent<Button>().enabled = true;
            Animator cardAnim = card.GetComponent<Animator>();
            cardAnim.enabled = true;
            cardAnim.Play("Close");
            yield return new WaitForSeconds(0.4f);
            cardAnim.enabled = false;
            card.GetComponent<Image>().sprite = cardBack;
            yield return new WaitForSeconds(0.1f);
        }
        userInput = new List<GameObject>();
        SetButtonsEnabled(true);
        yield break;
    }

    public void Back()
    {
        SceneManager.LoadScene("Map");
    }

    public void Redo() 
    {
        redo.SetActive(false);
        GenerateOrder();
        StartMinigame();
    }
}
