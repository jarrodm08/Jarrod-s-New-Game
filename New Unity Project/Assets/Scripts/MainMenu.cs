using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject mainMenu;
    private GameObject optionsMenu;

    void Start()
    {
        mainMenu = Camera.main.transform.Find("Canvas").Find("eMainMenu").gameObject;
        optionsMenu = Camera.main.transform.Find("Canvas").Find("eOptionsMenu").gameObject;
        LoadClouds();
        LoadButtons();
        LoadSettings();
    }

    void Update()
    {
        MoveClouds();
    }



    private void LoadSettings()
    {
        Slider musicSlider = optionsMenu.transform.Find("VolumeSlider").GetComponent<Slider>();
        musicSlider.value = FindObjectOfType<SessionData>().musicVolume;
        musicSlider.onValueChanged.AddListener(FindObjectOfType<MusicManager>().ChangeVolume);
    }

    public void SaveDataButton()
    {
        SaveManager.SaveData(FindObjectOfType<SessionData>().GetComponent<SessionData>());
    }

    private Button[] menuButtons = new Button[4];
    private void LoadButtons()
    {
        //Get the menu buttons into an array
        menuButtons[0] = mainMenu.transform.Find("PlayBtn").GetComponent<Button>();
        menuButtons[1] = mainMenu.transform.Find("OptionsBtn").GetComponent<Button>();
        menuButtons[2] = mainMenu.transform.Find("QuitBtn").GetComponent<Button>();
        menuButtons[3] = optionsMenu.transform.Find("BackBtn").GetComponent<Button>();
        //Set the listeners for the menu buttons
        menuButtons[0].onClick.AddListener(PlayButton);
        menuButtons[1].onClick.AddListener(OptionsButton);
        menuButtons[2].onClick.AddListener(QuitButton);
        menuButtons[3].onClick.AddListener(BackButton);
    }

    private void PlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    private void OptionsButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    private void QuitButton()
    {
        Application.Quit();
    }

    private void BackButton()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    #region Clouds
    private GameObject cloudA;
    private Vector2 startPos;
    private float scrollSpeed = 0.5f;

    private void LoadClouds()
    {
        cloudA = Camera.main.transform.Find("Canvas").Find("Clouds").gameObject;
        startPos = cloudA.transform.position;
    }

    private void MoveClouds()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed,17.7f); // Number 17.7f depends on size of image, adjust to a precise number where the image repeats seemlessly
        cloudA.transform.position = startPos + Vector2.right * newPos;
    }
    #endregion
}
