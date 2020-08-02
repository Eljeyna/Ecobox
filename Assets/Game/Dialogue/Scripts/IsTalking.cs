﻿using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IsTalking : MonoBehaviour
{
    public GameDirector game;
    public string dialogue_file;
    public int _currentLine;
    public bool isTalking = true;
    public bool dialogueStart = false;
    public bool controlAfter = true;
    public GameObject talkButton;

    public string speakerName;

    private TextAsset text = null;
    private Dialogue[] dialogue;
    private int dialogue_count;
    private int answer_count;

    void OnGUI()
    {
        if (dialogueStart)
        {
            if (!game.dialogue_box.activeSelf)
            {
                game.SetSpeaker(gameObject, null);
                game.StartDialogue();
                return;
            }

            if (text == null)
            {
                SetDialogue(null);
            }

            game.dialogue_box_name.text = dialogue[_currentLine].name;
            game.dialogue_box_text.text = dialogue[_currentLine].text;

            if (dialogue[_currentLine].answers != null)
            {
                for (int i = 0; i < dialogue[_currentLine].answers.Length; i++)
                {
                    if (GUI.Button(new Rect(
                            Screen.width * 0.5f - (Screen.width * 0.15f),
                            Screen.height - (Screen.height * 0.235f) + (Screen.height * 0.11f) * i,
                            Screen.width * 0.3f,
                            Screen.height * 0.1f),
                            dialogue[_currentLine].answers[i].answer_text,
                            game.dialogueButton)
                        )
                    {
                        if (dialogue[_currentLine].answers[i].dialogue_file.Equals("0"))
                        {
                            IsTalkingDone();
                        }
                        else
                        {
                            SetDialogue(dialogue[_currentLine].answers[i].dialogue_file);
                        }
                    }
                }
            }
            else
            {
                if (GUI.Button(new Rect(
                        0,
                        0,
                        Screen.width,
                        Screen.height),
                        "",
                        game.invisibleButton)
                    )
                {
                    if (dialogue.Length - 1 == _currentLine)
                    {
                        IsTalkingDone();
                    }
                    else
                    {
                        _currentLine += 1;
                    }
                }
            }
        }
    }

    public void SetDialogue(string file)
    {
        if (file == null)
        {
            text = (TextAsset)Resources.Load("Dialogues/" + SceneManager.GetActiveScene().name + "/" + dialogue_file + " RU");
        }
        else
        {
            text = (TextAsset)Resources.Load("Dialogues/" + SceneManager.GetActiveScene().name + "/" + file + " RU");
        }
        string line = text.text.Replace(": ", ":");
        string[] lines = line.Split('\n');
        
        _currentLine = 0;
        dialogue_count = 0;
        answer_count = 0;

        int count_answers = 0;
        int i = 0;
        int j = 0;

        while (i < lines.Length - 1)
        {
            if (lines[i][0] == '*')
            {
                count_answers++;
            }
            i++;
        }

        dialogue = new Dialogue[lines.Length - 1];
        i = 0;

        if (count_answers > 0)
        {
            while (i < lines.Length - 1)
            {
                dialogue[dialogue_count] = new Dialogue();
                if (lines[i][0] == '*')
                {
                    dialogue[dialogue_count].answers = new Answer[count_answers];
                    while (i < lines.Length - 1 && lines[i][0] == '*')
                    {
                        dialogue[dialogue_count].answers[answer_count] = new Answer();
                        int index = lines[i].IndexOf('>');
                        dialogue[dialogue_count].answers[answer_count].answer_text = lines[i].Substring(3, index - 4);
                        dialogue[dialogue_count].answers[answer_count].dialogue_file = lines[i].Substring(index + 1, lines[i].Length - 2 - index);
                        i++;
                        answer_count++;
                    }
                }
                else
                {
                    j = 0;
                    while (lines[i][j] != ':')
                    {
                        dialogue[dialogue_count].name += lines[i][j];
                        j++;
                    }
                    dialogue[dialogue_count].text = lines[i].Substring(j + 1);
                }

                i++;
                dialogue_count++;
            }
        }
        else
        {
            while (i < lines.Length - 1)
            {
                j = 0;
                dialogue[dialogue_count] = new Dialogue();
                while (lines[i][j] != ':')
                {
                    dialogue[dialogue_count].name += lines[i][j];
                    j++;
                }
                dialogue[dialogue_count].text = lines[i].Substring(j + 1);

                i++;
                dialogue_count++;
            }
        }
    }

    public void IsTalkingDone()
    {
        _currentLine = 0;
        dialogueStart = false;
        game.SetControlAfter(controlAfter);
        game.StopDialogue();
    }

    public class Dialogue
    {
        public string text;
        public string name;
        public Answer[] answers;
    }

    public class Answer
    {
        public string answer_text;
        public string dialogue_file;
    }
}