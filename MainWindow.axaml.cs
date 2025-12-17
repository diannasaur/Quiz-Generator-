/*
this file initalizes the avalonia gui and methods
it includes 5 seperate methods (ProcessQuiz, generateNewQuiz, DisplayQuestion, GetSelectedAnswer, and NextButtonClick)  
before each method, I added the purpose of them and how they work
i put all my main code in this file, since it also initalizes the avalonia GUI 
*/

using System;
using System.IO;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AvaloniaGUI_Quiz;

public partial class MainWindow : Window
{
    //original 50 question quiz
    public List<CQuiz> quizList = new List<CQuiz>();
    //duplicate 50 question quiz
    public List<CQuiz> quizListDuplicate = new List<CQuiz>();
    //20 question quiz
    public List<CQuiz> selectedQuizList = new List<CQuiz>();
    //current question number
    public int currentQuestionNum = 0;
    //total score of quiz
    public int score = 0;
    //user's selected answer
    public string selectedAnswer;


    public MainWindow()
    {        
        InitializeComponent();
        ProcessQuiz();
        GenerateNewQuiz();
    }

    //this method reads the Questions file, processes the data into CQuiz objects and add them to the main quiz list
    //it adds all the lines to a line list and processes the lines by 3 into CQuiz objects
    public void ProcessQuiz()
    {
         try //trys executing the code below
        {
            string filePath = "Questions.txt";
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = "";
                List<string> lines = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }

                for (int i = 0; i < lines.Count; i += 3)
                {
                    string question = lines[i];
                    string optionsLine = lines[i + 1];
                    string correctAnswer = lines[i + 2];
                

                    string[] options = optionsLine.Split(';');
                    List<string> optionsList = new List<string>();

                    for (int j = 0; j < options.Length; j++)
                    {
                        optionsList.Add(options[j]);
                    }

                    CQuiz quizQuestion = new CQuiz(question, optionsList, correctAnswer);
                    quizList.Add(quizQuestion);
                }
            }
        }
        catch (FileNotFoundException) //catches if the file is not found
        {
            Console.WriteLine("Questions.txt was not found.");
        }
        catch (Exception ex) //catches any other exceptions
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    //this method generates a new 20 question quiz
    public void GenerateNewQuiz()
    {
        //for each new quiz, we first clear the selected quiz list and dulipcate list
        selectedQuizList.Clear();
        quizListDuplicate.Clear();

        //resets question number and score for each new quiz made
        currentQuestionNum = 0;
        score = 0;

        //refills dulipcate list
        foreach (CQuiz q in quizList)
        {
            quizListDuplicate.Add(q);
        }

        //generates a random number, and then we use that number to add to the current quiz list
        Random rand = new Random();

        while (selectedQuizList.Count < 20)
        {            
            int index = rand.Next(quizListDuplicate.Count);
            selectedQuizList.Add(quizListDuplicate[index]);
            quizListDuplicate.RemoveAt(index);
        }

        NextButton.Content = "Next";

        DisplayQuestion();
    }

    //this method displays a question, its choices, the current score, and the current question number  
    public void DisplayQuestion()
    {
        var question = selectedQuizList[currentQuestionNum];

        QuestionText.Text = question.Question;

        OptionA.Content = question.Options[0];
        OptionB.Content = question.Options[1];
        OptionC.Content = question.Options[2];
        OptionD.Content = question.Options[3];

        OptionA.IsVisible = true;
        OptionB.IsVisible = true;
        OptionC.IsVisible = true;
        OptionD.IsVisible = true;

        OptionA.IsChecked = false;
        OptionB.IsChecked = false;
        OptionC.IsChecked = false;
        OptionD.IsChecked = false;

        DisplayScore.Text = $"Total Score: {score}";
        FooterText.Text = $"Answering question {currentQuestionNum + 1} of 20";
    }

    //this method checks each button to get the selected answer and returns it as a string
    public string GetSelectedAnswer()
    {
        if (OptionA.IsChecked == true) 
        {
            return OptionA.Content.ToString();
        }
        else if (OptionB.IsChecked == true) 
        {
            return OptionB.Content.ToString();
        }
        else if (OptionC.IsChecked == true)
        {
            return OptionC.Content.ToString();
        }
        else if (OptionD.IsChecked == true) 
        {
            return OptionD.Content.ToString();
        }
        else
        {
            return null;
        }

    }

    //this method is for when the next button is clicked
    private void NextButtonClick(object? sender, RoutedEventArgs e)
    {
        //if the next button is set to restart, then it will create a new quiz
        if (NextButton.Content.ToString() == "Restart")
        {
            GenerateNewQuiz();
            return;
        }

        //get selected answer
        string selected = GetSelectedAnswer();

        //if the user doesn't select an answer, then it displays a error message
        if (selected == null)
        {
            FooterText.Text = "Please select an answer.";
            return;
        }

        //is the selected answer is the same as the correct answer, then the score updates
        if (selected == selectedQuizList[currentQuestionNum].CorrectAnswer)
        {
            score++;
        }

        currentQuestionNum++;
        
        //updates text when quiz is completed and displays the final score
        if (currentQuestionNum >= 20)
        {
            DisplayScore.Text = "";
            QuestionText.Text = "Quiz Complete!";
            OptionA.IsVisible = false;
            OptionB.IsVisible = false;
            OptionC.IsVisible = false;
            OptionD.IsVisible = false;
            FooterText.Text = $"Final Score: {score} out of 20!";
            NextButton.Content = "Restart";
            return;
         }

        DisplayQuestion();
    }

}