using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class SingingFrogs : Minigame
{
    public Dictionary<string, Sprite[]> frogPhases = new Dictionary<string, Sprite[]>();
    public List<Sprite> frogSprites;
    public Image informationPanelImage;
    public TMP_Text infoTekst;
    public int maxFrogs;
    public bool started = false;
    public int lastTap = 0;
    public GameObject redo;
    public GameObject back;

    public override void Start()
    {
        LoadFrogDic();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        GenerateOrder();
        StartMinigame();
    }

    // Start is called before the first frame update
    public override void StartMinigame()
    {
        SetButtonsEnabled(false);
    }
    
    // Update is called once per frame
    public void Update()
    {
        if (Input.touchCount > lastTap && !started) 
        {
            started = true;
            informationPanelImage.enabled = false;
            StartSinging();
        }
    }

    public void Redo() 
    {
        SetButtonsEnabled(false);
        gameInput = new List<GameObject>();
        informationPanelImage.enabled = false;
        GenerateOrder();
        StartSinging();
    }

    public override void GenerateOrder() 
    {
        
        int index = Random.Range(0, frogPhases.Count);
        if (gameInput.Count != 0)
        {
            while (frogPhases.ElementAt(index).Value[0].name == gameInput[gameInput.Count - 1].name)
                index = Random.Range(0, frogPhases.Count);

            gameInput.Add(allInputButtons.Find((x) => x.name == frogPhases.ElementAt(index).Value[0].name));
        }
        else 
        {
            gameInput.Add(allInputButtons.Find((x) => x.name == frogPhases.ElementAt(index).Value[0].name));
        }
        userInputCap = gameInput.Count;
    }

    public void LoadFrogDic() 
    {
        for (int i = 0; i < frogSprites.Count; i+=3)
        {
            frogPhases.Add(frogSprites[i].name, new Sprite[] { frogSprites[i], frogSprites[i + 1], frogSprites[i + 2] });
        }
    }

    public void StartSinging()
    {
        redo.SetActive(false);
        userInput = new List<GameObject>();
        StartCoroutine(Singing());
    }

    private IEnumerator Singing() 
    {
        
        yield return new WaitForSeconds(0.5f);
        infoTekst.text = "Let op! De kikkers zingen.";
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject frog in gameInput) 
        {
            frog.GetComponent<Animator>().Play("Sing");
            frog.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(1.2f);
        }
        SetButtonsEnabled(true);
        infoTekst.text = "Jouw Beurt!";
        yield break;
    }

    public override void CheckUserOrder() 
    {
        for (int i = 0; i < userInput.Count; i++)
        {
            if (gameInput[i].name != userInput[i].name)
            {
                infoTekst.text = "Fout!";
                FlashAllFrogs("Dizzy");
                userInput = new List<GameObject>();
                CancelInvoke(nameof(CheckUserOrder));
                StartCoroutine(Singing());
                return;
            }
        }
        infoTekst.text = "Correct!";
        userInput = new List<GameObject>();
        FlashAllFrogs("Sing");

        CancelInvoke(nameof(CheckUserOrder));
        if (gameInput.Count < maxFrogs)
        {
            GenerateOrder();
            StartCoroutine(Singing());
        }
        else
        {
            infoTekst.text = "Gewonnen!";
            informationPanelImage.enabled = true;
            redo.SetActive(true);
            back.SetActive(true);
        }
        return;
    }

    private void FlashAllFrogs(string anim)
    {
        foreach (GameObject frog in allInputButtons)
        {
            frog.GetComponent<Animator>().Play(anim);
        }
    }

    public void Back()
    {
        SceneManager.LoadScene("Map");
    }
}
