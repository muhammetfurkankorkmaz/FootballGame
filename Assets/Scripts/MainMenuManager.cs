using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject creditsScreen;
    [SerializeField] GameObject howToPlayScreen;

    GameObject currentScreen;

    public void StartButton()
    {
        SceneManager.LoadScene("CharacterSelection");
    }
    public void CreditsButton()
    {
        creditsScreen.SetActive(true);
        currentScreen = creditsScreen;
    }
    public void HowToPlayButton()
    {
        howToPlayScreen.SetActive(true);
        currentScreen = howToPlayScreen;
    }
    public void ReturnButton()
    {
        currentScreen.SetActive(false);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}//Class
