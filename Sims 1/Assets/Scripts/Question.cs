using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Question 
{
    // Start is called before the first frame update
    public string statementText { get; set; }
    public string image { get; set; }
    public string source { get; set; }
    public List<string> answersOptions { get; set; }
    public string correctAnswer { get; set; }
    public int resolutionTime { get; set; }
    public int score { get; set; }

    #region Getters
    public string GetStatementText()
    {
        return statementText;
    }

    public string GetStatementImage()
    {
        return image;
    }

    public string GetSource()
    {
        return source;
    }

    public List<string> GetAnswersOptions()
    {
        return answersOptions;
;
    }

    public string GetCorrectAnswer()
    {
        return correctAnswer;
    }

    public int GetResolutionTime()
    {
        return resolutionTime;
    }

    public int GetScore()
    {
        return score;
    }

    public List<string> GetWrongAnswers()
    {
        return answersOptions.Where(x => correctAnswer != x).ToList();
    }
    #endregion


    #region Setters
    public void SetStatementText(string text)
    {
        statementText = text;
    }

    public void SetStatementImage(string imageName)
    {
        image = imageName;
    }

    public void SetSource(string questionSource)
    {
        source = questionSource;
    }

    public void SetAnswersOptions(List<string> ans)
    {
        answersOptions = ans;
        ;
    }

    public void SetCorrectAnswer(string correctAns)
    {
        correctAnswer = correctAns;
    }

    public void SetResolutionTime(int time)
    {
        resolutionTime = time;
    }

    public void SetScore(int points)
    {
        score = points;
    }

    #endregion
}
