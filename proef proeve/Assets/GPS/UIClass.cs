using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIClass : MonoBehaviour
{
    [SerializeField] GameObject hubUI;
    [SerializeField] GameObject hubButtons;
    [SerializeField] GameObject miniGames;
    [SerializeField] GameObject hudUI;
    //[SerializeField] GameObject quitGameUI;

    private void Start()
    {
        MainMenu();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void HUB()
    {
        hubUI.SetActive(true);
        hubButtons.SetActive(true);
        miniGames.SetActive(false);
        hudUI.SetActive(false);
       // quitGameUI.SetActive(false);
    }

    public void MainMenu()
    {
        hubUI.SetActive(false);
        hubButtons.SetActive(false);
        miniGames.SetActive(false);
        hudUI.SetActive(true);
       // quitGameUI.SetActive(false);
    }

    public void OpenGames()
    {
        hubUI.SetActive(true);
        hubButtons.SetActive(false);
        miniGames.SetActive(true);
        hudUI.SetActive(false);
       // quitGameUI.SetActive(false);
    }

    /*
    public void QuitGame()
    {
        hubUI.SetActive(true);
        hubButtons.SetActive(false);
        miniGames.SetActive(false);
        hudUI.SetActive(false);
        quitGameUI.SetActive(true);
    }

    public void QuitGameComfirm()
    {

    }
    */

    public void PlayGame1()
    {
        SceneManager.LoadScene("SingingFrogs");
    }
    public void PlayGame2()
    {
        SceneManager.LoadScene("Memory");
    }
    public void PlayGame3()
    {
        SceneManager.LoadScene("Tracks");
    }

}
