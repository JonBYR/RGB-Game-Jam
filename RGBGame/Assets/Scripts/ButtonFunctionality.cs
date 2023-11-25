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
    // Start is called before the first frame update
    void Start()
    {
        var route = GetComponent<UIDocument>().rootVisualElement;
        MainButton = route.Q<Button>("MainMenu");
        MainButton.clicked += LoadMain;
        QuitButton = route.Q<Button>("Quit");
        QuitButton.clicked += QuitGame;
        LoadGame = route.Q<Button>("LoadButton");
        LoadGame.clicked += LoadLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadMain()
    {
        SceneManager.LoadScene("Main Menu");
    }
    void QuitGame()
    {
        Application.Quit();
    }
    void LoadLevel()
    {
        SceneManager.LoadScene("LevelGeneration");
    }
}
