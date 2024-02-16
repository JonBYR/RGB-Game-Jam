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
    public Button secondQuit;
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

        var route = GetComponent<UIDocument>().rootVisualElement; //UI Document used to display start menu for the game
        start = route.Q<VisualElement>("StartingMenu"); //Different menu types
        credit = route.Q<VisualElement>("Credits");
        end = route.Q<VisualElement>("EndState"); //for when the game ends
        start.visible = true; //starting menu is visible first
        credit.visible = false;
        end.visible = false;
        MainButton = route.Q<Button>("MainMenu");
        MainButton.clicked += LoadMain; //event to load the game
        backButton = route.Q<Button>("BuckButton");
        backButton.clicked += DisplayStart; //event to display start menu, a function in this script
        credits = route.Q<Button>("Controls");
        credits.clicked += DisplayCred; //event to display credits by setting credits UI to true
        startGame = route.Q<Button>("StartGame"); //Button associated with starting game
        startGame.clicked += Load;
        QuitButton = route.Q<Button>("Quit"); //button assosciated with quitting game
        QuitButton.clicked += QuitGame; //event to quit game
        secondQuit = route.Q<Button>("Quit1");
        secondQuit.clicked += QuitGame;
        goldText = route.Q<Label>("GoldText"); //label associated with game
        goldText.text = "You have salvaged: " + PlayerController.amountOfGold.ToString();
        stateText = route.Q<Label>("SceneState");
        if (SceneManager.GetActiveScene().name == "WinScene") { end.visible = true;  credit.visible = false; start.visible = false; stateText.text = "You Won!"; } //end screen displayed with relevant text
        else if (SceneManager.GetActiveScene().name == "GameOver") { end.visible = true; credit.visible = false; start.visible = false; stateText.text = "Game Over!"; } //end UI menu is displayed if the scene transitions to a win/game over
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
