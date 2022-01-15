using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionModeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject popupExit;

    [SerializeField]
    private UnityEngine.UI.Button muteBtn;

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    // Start is called before the first frame update
    void Start()
    {
        InitializeIntroduction();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeIntroduction()
    {
        ActivateModeBtn();

        this.gameObject.GetComponent<SoundManagerScript>().StartSound();
        this.gameObject.GetComponent<SoundManagerScript>().PlaySound("Environment");

        IniteMuteBtnSprite();
    }

    public void IniteMuteBtnSprite()
    {
        if (AudioListener.pause)
        {
            muteBtn.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load("UI Components/Header_MuteBtn_Off", typeof(Sprite)) as Sprite;
        }
        else
        {
            muteBtn.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load("UI Components/Header_MuteBtn_On", typeof(Sprite)) as Sprite;
        }
    }

    #region Mode Btns
    public void ActivateModeBtn()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        //UnityEngine.UI.Button introBtn = GameObject.Find("IntroductionModeBtn").GetComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button gameBtn = GameObject.Find("GameModeBtn").GetComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button labBtn = GameObject.Find("LabModeBtn").GetComponent<UnityEngine.UI.Button>();

        //if (sceneName == "Introduction")
        //{
        //    SetBtnColor(labBtn, Color.yellow);
        //}
        //else if(sceneName == "Game")
        //{
        //    SetBtnColor(gameBtn, Color.yellow);
        //}
        //else
        //{
        //    ResetBtnColor(gameBtn);
        //    ResetBtnColor(labBtn);
        //}


    }

    private void ResetBtnColor(UnityEngine.UI.Button btn)
    {
        UnityEngine.UI.ColorBlock colorBlock = btn.colors;
        colorBlock.normalColor = Color.white;
        btn.colors = colorBlock;
    }

    private void SetBtnColor(UnityEngine.UI.Button btn, Color color)
    {
        UnityEngine.UI.ColorBlock colorBlock = btn.colors;
        colorBlock.normalColor = color;
        btn.colors = colorBlock;
    }
    #endregion

    #region Close App
    public void QuitApplication(bool popup = false)
    {
        if (popup)
        {
            Transform popupText = popupExit.GetComponent<Transform>().Find("Image").Find("Text");
            popupText.GetComponent<UnityEngine.UI.Text>().text = "Tem certeza que deseja voltar ao menu?";
            popupExit.SetActive(true);
            return;
        }

        Debug.Log("Closed application");
        Application.Quit();
    }
    #endregion

    public void DisablePopup(GameObject popup)
    {
        popup.SetActive(false);
    }

    public void changeScene(string sceneName)
    {
        if (sceneName == "Game") {
            SceneManager.LoadScene("Game");
        }
        else if (sceneName == "Introduction")
        {
            SceneManager.LoadScene("Introduction");
        }
        else if (sceneName == "Menu")
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void OnMinimizeButtonClick()
    {
        ShowWindow(GetActiveWindow(), 2);
    }

    public void StartClickSound()
    {
        this.gameObject.GetComponent<SoundManagerScript>().PlaySound("Click");
    }
}
