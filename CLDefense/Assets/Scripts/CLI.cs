﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CLI : MonoBehaviour {
    [SerializeField]
    private int _maxLinesCount = 20;

    [SerializeField]
    private Text _line;
    private Queue<Text> _lines = new Queue<Text>();
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private InputField _inputField;

    private bool _isInit = false;
    private Dictionary<string, Action<string>> _commands;

    private void Start() {
        Bind();
        _inputField.text = "";
        _inputField.ActivateInputField();
        _inputField.onEndEdit.AddListener(delegate { OnEndEdit(); });
        DisplayMessage(Utils.LoadTextFromFile("greeting.txt"));
    }

    private void Bind() {
        if(_isInit)
            return;
        _commands = new Dictionary<string, Action<string>>();
        _commands.Add("-clear", Clear);
        _commands.Add("-help", Help);
        _commands.Add("start", GameStart);
    }

    #region commands
    private void GameStart(string obj) {

    }

    private void Help(string obj) {
        DisplayMessage(Utils.LoadTextFromFile("help.txt"));
    }

    private void Clear(string obj) {
        foreach(var line in _lines)
            line.text = "";
    } 
    #endregion

    private void OnEndEdit() {
        string cmd = _inputField.text;
        _inputField.text = "";
        _inputField.ActivateInputField();
        if(string.IsNullOrEmpty(cmd))
            return;
        Push(cmd);
        Interpret(cmd);
    }

    private void Push(string message) {
        Text current = _line;
        if(_lines.Count < _maxLinesCount) {
            current = Instantiate<Text>(_line);
            current.transform.SetParent(_content);
            current.transform.localScale = Vector3.one;
        } else {
            current = _lines.Dequeue();
        }
        _lines.Enqueue(current);
        current.transform.SetAsLastSibling();
        current.text = message;
    }

    //TODO: no idea what to do here
    private void Interpret(string cmd) {
        bool found = false;
        foreach(var command in _commands) {
            if(cmd.StartsWith(command.Key)) {
                command.Value(cmd);
                found = true;
            }
        }
        if(!found)
            Push(string.Format(">> Unrecognized command '{0}'. Type -help for list of available commands.", cmd));
    }

    private void DisplayMessage(string message) {
        using(StringReader reader = new StringReader(message)) {
            string line;
            while((line = reader.ReadLine()) != null)
                Push(line);
        }
    }
}
    
