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
    

    [SerializeField]
    private UnityEngine.UI.Text statementTextbox;

    [SerializeField]
    private UnityEngine.UI.Text questionTitleTextbox;

    [SerializeField]
    private UnityEngine.UI.Text scoreTextbox;

    [SerializeField]
    private UnityEngine.UI.Text timeTextbox;

    [SerializeField]
    private UnityEngine.UI.Text currentWrongAnswersTextbox;

    [SerializeField]
    private UnityEngine.UI.Button aBtn;
    [SerializeField]
    private UnityEngine.UI.Button bBtn;
    [SerializeField]
    private UnityEngine.UI.Button cBtn;
    [SerializeField]
    private UnityEngine.UI.Button dBtn;

    [SerializeField]
    private UnityEngine.UI.Button hintBtn;

    [SerializeField]
    private UnityEngine.UI.Button passBtn;

    [SerializeField]
    private UnityEngine.UI.Button addTimeBtn;

    [SerializeField]
    private GameObject popupCheckAnswers;

    [SerializeField]
    private GameObject popupSummary;

    [SerializeField]
    private GameObject popupExit;

    [SerializeField]
    private GameObject questionImage;

    private const int NOFQUESTIONS = 8;
    private const int NOFANSWEROPTS = 4;
    private const int NOFWRONGQUESTIONS = 3;
    private const int NOFHINTS = 2;
    private const float DISABLEDOPACITY = 0.1490196f;
    private const float ENABLEDOPACITY = 1f;
    private const int ADDTIME = 30;

    private List<Question> sessionQuestions;
    private Question currentQuestion;
    private int currentQIndex = 1;

    [SerializeField]
    private TextAsset questionsFile;

    private int playerScore = 0;
    private int resolutionTime = 0;
    private int wrongQuestions = 0;
    private int rightQuestions = 0;

    private float timer = 0.0f;

    private bool isPopupOn = false;

    private List<UnityEngine.UI.Button> optionButtons;

    // Start is called before the first frame update
    void Start()
    {
        ActivateModeBtn();
        InitSessionQuestions();
        InitPlayerInfo();
        optionButtons = new List<UnityEngine.UI.Button>() { aBtn, bBtn, cBtn, dBtn };
    }

    // Update is called once per frame
    void Update()
    {
        TickTimer();

        if (wrongQuestions >= NOFWRONGQUESTIONS)
        {
            SetPopup(popupCheckAnswers, false, "");
            SetPopup(popupSummary, true, new List<string>() { scoreTextbox.text, rightQuestions + "/" + NOFQUESTIONS, wrongQuestions + "/" + NOFWRONGQUESTIONS });
        }
    }

    /// <summary>
    /// Atualiza o tempo da questão atual
    /// </summary>
    public void TickTimer()
    {
        if (isPopupOn)
        {
            return;
        }

        timer += Time.deltaTime;
        int seconds = (int)timer % 120;

        if (resolutionTime - seconds >= 0)
        {
            timeTextbox.text = resolutionTime - seconds + "";
        }
        else
        {
            UpdateScore(-currentQuestion.GetScore());
            IncreaseWrongAnswer();
            CallNextQuestion();
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
        colorBlock.selectedColor = Color.white;
        btn.colors = colorBlock;
    }

    private void SetBtnColor(UnityEngine.UI.Button btn, Color color)
    {
        UnityEngine.UI.ColorBlock colorBlock = btn.colors;
        colorBlock.normalColor = color;
        colorBlock.selectedColor = color;
        btn.colors = colorBlock;
    }
    #endregion

    #region Close App
    public void QuitApplication(bool popup = false)
    {
        if (popup)
        {
            SetPopup(popupExit, true, "Tem certeza que deseja sair?");
            return;
        }
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

        foreach (Question q in questions)
        {
            Debug.Log($"Score das questoes {q.GetScore()}");
        }

        return questions;
    }

    #region Init methods
    public void InitSessionQuestions()
    {
        List<Question> questions = LoadQuestions();
        sessionQuestions = GetRandomElements<Question>(questions, NOFQUESTIONS);

        if(sessionQuestions != null)
        {
            SetQuestionOnWindow(sessionQuestions[0], currentQIndex.ToString());
        }
    }

    public void InitPlayerInfo()
    {
        this.scoreTextbox.text = "0";
        this.timeTextbox.text = "0";
        this.currentWrongAnswersTextbox.text = "0/3";
    }
    #endregion

    public List<T> GetRandomElements<T>(IEnumerable<T> list, int elementsCount)
    {
        return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
    }

    public void CallNextQuestion()
    {
        currentQIndex++;

        SetQuestionOnWindow(sessionQuestions[currentQIndex - 1], currentQIndex.ToString());

        foreach (UnityEngine.UI.Button btn in optionButtons)
        {
            ResetBtnColor(btn);
            EnableBtn(btn);
        }

        SetPopup(popupCheckAnswers, false, "");
    }

    public void SetQuestionOnWindow(Question q, string qNumber)
    {
        if (q != null)
        {
            statementTextbox.text = q.GetStatementText();

            questionTitleTextbox.text = "PERGUNTA " + qNumber;

            List<string> answerOptions = GetRandomElements<string>(q.GetAnswersOptions(), NOFANSWEROPTS);

            if (answerOptions != null)
            {
                aBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = answerOptions[0];

                bBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = answerOptions[1];

                cBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = answerOptions[2];

                dBtn.GetComponentInChildren<UnityEngine.UI.Text>().text = answerOptions[3];
            }

            currentQuestion = q;

            timer = 0.0f;

            resolutionTime = q.resolutionTime;

            if (q.GetStatementImage() == null)
            {
                questionImage.SetActive(false);
            }
            else
            {

                Debug.Log(q.GetStatementImage());
                Sprite image = Resources.Load(q.GetStatementImage(), typeof(Sprite)) as Sprite;
                questionImage.GetComponent<SpriteRenderer>().sprite = image;
                questionImage.SetActive(true);
            }     

        }
    }

    public void UpdateScore(int score)
    {

        int tempPlayerScore = playerScore + score;
        playerScore = (tempPlayerScore < 0) ? 0 : tempPlayerScore;

        // Atualiza o score do jogador
        scoreTextbox.text = playerScore.ToString();
    }

    public void IncreaseWrongAnswer()
    {
        wrongQuestions += 1;
        currentWrongAnswersTextbox.text = $"{wrongQuestions}/{NOFWRONGQUESTIONS}";
    }

    public void IncreaseRightAnswer()
    {
        rightQuestions += 1;
    }

    public void CheckOnClickAnswer(UnityEngine.UI.Button btn)
    {
        if(IsCorrectAnswer(btn))
        {
            SetBtnColor(btn, Color.green);
            UpdateScore(currentQuestion.GetScore());
            IncreaseRightAnswer();
            SetPopup(popupCheckAnswers, true, "Parabéns, você acertou esta questão!");
        }
        else
        {
            SetBtnColor(btn, Color.red);

            UpdateScore(-currentQuestion.GetScore());
            IncreaseWrongAnswer();

            SetPopup(popupCheckAnswers, true, "Poxa, você errou esta questão!");
        }

        if (currentQIndex == 8)
        {
            SetPopup(popupCheckAnswers, false, "");
            SetPopup(popupSummary, true, new List<string>() { scoreTextbox.text, rightQuestions + "/" + NOFQUESTIONS, wrongQuestions + "/" + NOFWRONGQUESTIONS });
        }
    }

    public void SetPopup(GameObject popup, bool active, string popupMessage)
    {
        isPopupOn = active;
        Transform popupText = popup.GetComponent<Transform>().Find("Image").Find("Text");
        popupText.GetComponent<UnityEngine.UI.Text>().text = popupMessage;
        popup.SetActive(active);
    }

    public void SetPopup(GameObject popup, bool active, List<string> popupMessage)
    {

        isPopupOn = active;
        Transform popupScoreText = popup.GetComponent<Transform>().Find("PopupImage").Find("ScoreImage").Find("ScoreText");
        popupScoreText.GetComponent<UnityEngine.UI.Text>().text = popupMessage[0];

        Transform popupRightAnswerText = popup.GetComponent<Transform>().Find("PopupImage").Find("RightAnswerImage").Find("RightAnswerText");
        popupRightAnswerText.GetComponent<UnityEngine.UI.Text>().text = popupMessage[1];

        Transform popupWrongAnswerText = popup.GetComponent<Transform>().Find("PopupImage").Find("WrongAnswerImage").Find("WrongAnswerText");
        popupWrongAnswerText.GetComponent<UnityEngine.UI.Text>().text = popupMessage[2];
        popup.SetActive(active);
    }

    public void DisablePopup(GameObject popup)
    {
        isPopupOn = false;
        popup.SetActive(false);
    }

    public bool IsCorrectAnswer(UnityEngine.UI.Button btn)
    {
        return btn.GetComponentInChildren<UnityEngine.UI.Text>().text == currentQuestion.GetCorrectAnswer();
    }

    private void PrintCollection(List<string> list)
    {
        foreach (string item in list)
        {
            Debug.Log(item);
        }
    }

    public void GiveHint()
    {
        List<string> wrongAnswers = GetRandomElements<string>(currentQuestion.GetWrongAnswers(), NOFHINTS);

        PrintCollection(wrongAnswers);

        foreach (string wrongAnswer in wrongAnswers)
        {
            if (aBtn.GetComponentInChildren<UnityEngine.UI.Text>().text == wrongAnswer)
            {
                DisableBtn(aBtn);
            }
            else if(bBtn.GetComponentInChildren<UnityEngine.UI.Text>().text == wrongAnswer)
            {
                DisableBtn(bBtn);
            }
            else if (cBtn.GetComponentInChildren<UnityEngine.UI.Text>().text == wrongAnswer)
            {
                DisableBtn(cBtn);
            }
            else if (dBtn.GetComponentInChildren<UnityEngine.UI.Text>().text == wrongAnswer)
            {
                DisableBtn(dBtn);
            }
        }

        DisableBtn(hintBtn);
    }

    public void AddMoreTime()
    {
        DisableBtn(addTimeBtn);
        resolutionTime += ADDTIME;
    }

    public void PassQuestion()
    {
        DisableBtn(passBtn);
        CallNextQuestion();
    }

    public void DisableBtn(UnityEngine.UI.Button btn)
    {
        btn.interactable = false;

        UnityEngine.UI.ColorBlock colorBlock = btn.colors;
        colorBlock.disabledColor = new Color(colorBlock.disabledColor.r, colorBlock.disabledColor.g, colorBlock.disabledColor.b, DISABLEDOPACITY);
        btn.colors = colorBlock;

        //Color btnColor = btn.GetComponentInChildren<UnityEngine.UI.Text>().color;
        //btnColor.a = DISABLEDOPACITY;
        //btn.GetComponentInChildren<UnityEngine.UI.Text>().color = btnColor;
    }

    public void EnableBtn(UnityEngine.UI.Button btn)
    {
        btn.interactable = true;

        //Color btnColor = btn.GetComponentInChildren<UnityEngine.UI.Text>().color;
        //btnColor.a = ENABLEDOPACITY;
        //btn.GetComponentInChildren<UnityEngine.UI.Text>().color = btnColor;
    }
}
