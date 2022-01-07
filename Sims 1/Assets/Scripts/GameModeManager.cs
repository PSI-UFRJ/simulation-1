using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private UnityEngine.UI.Text statementTextbox;

    [SerializeField]
    private UnityEngine.UI.Text questionTitleTextbox;

    [SerializeField]
    private UnityEngine.UI.Button aBtn;
    [SerializeField]
    private UnityEngine.UI.Button bBtn;
    [SerializeField]
    private UnityEngine.UI.Button cBtn;
    [SerializeField]
    private UnityEngine.UI.Button dBtn;

    private const int NOFQUESTIONS = 8;
    private const int NOFANSWEROPTS = 4;

    private List<Question> sessionQuestions;
    private Question currentQuestion;
    private int currentQIndex = 1;

    [SerializeField]
    private TextAsset questionsFile;


    void Start()
    {
        ActivateModeBtn();
        InitSessionQuestions();
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

    public List<Question> LoadQuestions()
    {
        List<Question> questions;

        questions = JsonConvert.DeserializeObject<List<Question>>(questionsFile.text);

        return questions;
    }

    public void InitSessionQuestions()
    {

        List<Question> questions = LoadQuestions();
        sessionQuestions = GetRandomElements<Question>(questions, NOFQUESTIONS);

        if(sessionQuestions != null)
        {
            SetQuestionOnWindow(sessionQuestions[0], currentQIndex.ToString());
        }
        
    }

    public List<T> GetRandomElements<T>(IEnumerable<T> list, int elementsCount)
    {
        return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
    }

    public void SetQuestionOnWindow(Question q, string qNumber)
    {
        if (q != null)
        {
            statementTextbox.text = q.GetStatementText();

            questionTitleTextbox.text = "Pergunta " + qNumber;

            List<string> answerOptions = GetRandomElements<string>(q.GetAnswersOptions(), NOFANSWEROPTS);

            if (answerOptions != null)
            {
                aBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = answerOptions[0];

                bBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = answerOptions[1];

                cBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = answerOptions[2];

                dBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = answerOptions[3];
            }

            currentQuestion = q;
        }
    }


    private void printCollection(List<string> list)
    {
        foreach (string item in list)
        {
            Debug.Log(item);
        }
    }
}
