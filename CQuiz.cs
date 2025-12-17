/*
this file creates a CQuiz class for the quiz questions
*/

using System;
using System.IO;
using System.Collections.Generic;

public class CQuiz
{
public string Question { get; set; }
public List<string> Options { get; set; }
public string CorrectAnswer { get; set; }

public CQuiz(string question, List<string> options, string correctAnswer)
    {
        Question = question;
        Options = options;
        CorrectAnswer = correctAnswer;
    }
}