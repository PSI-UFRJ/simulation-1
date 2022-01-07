using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductionModeManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ActivateModeBtn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Mode Btns
    public void ActivateModeBtn()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        //UnityEngine.UI.Button introBtn = GameObject.Find("IntroductionModeBtn").GetComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button gameBtn = GameObject.Find("GameModeBtn").GetComponent<UnityEngine.UI.Button>();
        UnityEngine.UI.Button labBtn = GameObject.Find("LabModeBtn").GetComponent<UnityEngine.UI.Button>();

        if (sceneName == "Introduction")
        {
            SetBtnColor(labBtn, Color.yellow);
        }
        else if(sceneName == "Game")
        {
            SetBtnColor(gameBtn, Color.yellow);
        }
        else
        {
            ResetBtnColor(gameBtn);
            ResetBtnColor(labBtn);
        }


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
    public void QuitApplication()
    {
        Debug.Log("Closed application");
        Application.Quit();
    }
    #endregion

    public void changeScene(string sceneName)
    {
        if (sceneName == "Game") {
            SceneManager.LoadScene("Game");
        }
        else if (sceneName == "Introduction")
        {
            SceneManager.LoadScene("Introduction");
        }
    }
}
