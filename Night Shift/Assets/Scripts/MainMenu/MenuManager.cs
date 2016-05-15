using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject creditsMenu;
    public GameObject storyMenu;


    public void SelectMainMenu() {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        storyMenu.SetActive(false);
    }

    public void SelectCreditsMenu() {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void SelectStoryMenu() {
        mainMenu.SetActive(false);
        storyMenu.SetActive(true);
    }

    public void LoadScene(string level) {
        SceneManager.LoadScene(level);
    }

    public void ApplicationQuit() {
        Application.Quit();
    }
}
