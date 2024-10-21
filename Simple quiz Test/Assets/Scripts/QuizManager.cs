using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public Questions[] categories; 
    private Questions selectedCategory;

    private int currentQuestionsIndex = 0;

    public TMP_Text questionText;
    public Image questionImage;
    public Button[] replyButtons;
    public TMP_Text scoreText; // Reference to score text UI element
   
    private int score = 0; // Variable to keep track of the score

    // Start is called before the first frame update
    void Start()
    {
        categorySelect(0);
        UpdateScoreText(); 
    }

    public void categorySelect(int categoryIndex)
    {
        selectedCategory = categories[categoryIndex];
        currentQuestionsIndex = 0;
        score = 0; // Reset score when a new category is selected
        UpdateScoreText(); // Update the score display
        DisplayQuestion();
    }

    public void DisplayQuestion()
    {
        if (selectedCategory == null || selectedCategory.questionList.Length == 0) return;

        var question = selectedCategory.questionList[currentQuestionsIndex];

        questionText.text = question.questionFormulation;
        questionImage.sprite = question.QuestionImage;

        for (int i = 0; i < replyButtons.Length; i++)
        {
            if (i < question.answers.Length) // Ensure there are enough answers
            {
                TMP_Text buttonText = replyButtons[i].GetComponentInChildren<TMP_Text>();
                buttonText.text = question.answers[i];
                replyButtons[i].gameObject.SetActive(true); // Show buttons that have answers
            }
            else
            {
                replyButtons[i].gameObject.SetActive(false); // Hide extra buttons
            }
        }
    }

    public void OnReplySelect(int replyIndex)
    {
        if (replyIndex == selectedCategory.questionList[currentQuestionsIndex].CorrectAnswer)
        {
            Debug.Log("Correct answer");
            score++; // Increment score for correct answer
        }
        else
        {
            Debug.Log("Wrong answer");
            score--; // Decrement score for wrong answer
        }

        currentQuestionsIndex++;

        if (currentQuestionsIndex < selectedCategory.questionList.Length)
        {
            DisplayQuestion();
        }
        else
        {
            Debug.Log("No more questions!");
         
        }

        UpdateScoreText(); // Update the score display after each answer
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
