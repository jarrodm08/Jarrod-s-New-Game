using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{ 
    void Start()
    {
        LoadUI();
        //LoadSettings();
    }

    void Update()
    {
        MoveClouds(); // Handles scrolling of clouds
    }

    private void LoadUI()
    {
        Transform canvas = Camera.main.transform.Find("Canvas");

        #region Menus
        GameObject mainMenu = canvas.Find("eMainMenu").gameObject;
        GameObject optionsMenu = canvas.Find("eOptionsMenu").gameObject;
        #endregion

        #region Clouds
        cloud = canvas.Find("Clouds").gameObject;
        cloudStartPos = cloud.transform.position;
        #endregion

        #region Buttons

        Button[] menuButtons = new Button[4];
        menuButtons = canvas.GetComponentsInChildren<Button>(true);
        for(int i = 0; i< menuButtons.Length;i++)
        {
            string name = menuButtons[i].name;
            menuButtons[i].onClick.AddListener(() => ClickMenuButton(name));
        }

        //Give functionality to each button
        void ClickMenuButton(string btnName)
        {
            if (btnName == "PlayBtn")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (btnName == "OptionsBtn")
            {
                mainMenu.SetActive(false);
                optionsMenu.SetActive(true);
            }
            else if (btnName == "QuitBtn")
            {
                Application.Quit();
            }
            else if (btnName == "BackBtn")
            {
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
        #endregion

        #region Settings
        Slider musicSlider = optionsMenu.GetComponentInChildren<Slider>(true);
        musicSlider.onValueChanged.AddListener(FindObjectOfType<MusicManager>().ChangeMusicVolume);
        musicSlider.value = GameData.sessionData.menuSettings.musicVolume;
        #endregion
    }

    #region Clouds
    private GameObject cloud;
    private Vector2 cloudStartPos;
    private float cloudSpeed = 0.5f;

    private void MoveClouds()
    {
        float newCloudPos = Mathf.Repeat(Time.time * cloudSpeed, 17.7f); // Number 17.7f depends on size of image
        cloud.transform.position = cloudStartPos + Vector2.right * newCloudPos;
    }
    #endregion



}
