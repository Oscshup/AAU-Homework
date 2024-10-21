using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class Question
{
    public string questionFormulation;
    public string[] answers;
    public int CorrectAnswer;
    public Sprite QuestionImage;
}


[CreateAssetMenu(fileName = "New Question Category", menuName = "Quiz/Questions")]


public class Questions : ScriptableObject
{
    public string category;
    public Question[] questionList; 
}
