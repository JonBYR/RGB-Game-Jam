using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class ButtonFunctionality : MonoBehaviour
{
    public Button MainButton;
    public Button QuitButton;
    public Button LoadGame;
    public Button startGame;
    public Label goldText;
    public Label stateText;
    public VisualElement start;
    public VisualElement credit;
    public VisualElement end;
    public Button backButton;
    public Button credits;
    // Start is called before the first frame update
    void Start()
    {

        var route = GetComponent<UIDocument>().rootVisualElement;
        start = route.Q<VisualElement>("StartingMenu");
        credit = route.Q<VisualElement>("Credits");
        end = route.Q<VisualElement>("EndState");
        start.visible = true;
        credit.visible = false;
        end.visible = false;
        MainButton = route.Q<Button>("MainMenu");
        MainButton.clicked += LoadMain;
        backButton = route.Q<Button>("BuckButton");
        backButton.clicked += DisplayStart;
        credits = route.Q<Button>("Controls");
        credits.clicked += DisplayCred;
        startGame = route.Q<Button>("StartGame");
        startGame.clicked += Load;
        QuitButton = route.Q<Button>("Quit");
        QuitButton.clicked += QuitGame;
        goldText = route.Q<Label>("GoldText");
        goldText.text = "You have salvaged: " + PlayerController.amountOfGold.ToString();
        stateText = route.Q<Label>("SceneState");
        if (SceneManager.GetActiveScene().name == "WinScene") { end.visible = true;  stateText.text = "You Won!"; }
        else if (SceneManager.GetActiveScene().name == "GameOver") { end.visible = true; stateText.text = "Game Over!"; }
        Time.timeScale = 0;
    }
    void LoadMain()
    {
        SceneManager.LoadScene("LevelGeneration");
    }
    void QuitGame()
    {
        Application.Quit();
    }
    void DisplayCred()
    {
        start.visible = false;
        credit.visible = true;
    }
    void DisplayStart()
    {
        start.visible = true;
        credit.visible = false;
    }
    void Load()
    {
        start.visible = false;
        Time.timeScale = 1.0f;
    }
}
