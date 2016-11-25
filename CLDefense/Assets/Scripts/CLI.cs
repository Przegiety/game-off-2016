using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TD;

public class CLI : MonoBehaviour {
    [SerializeField]
    private Color _regularColor;
    [SerializeField]
    private Color _errorColor;

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

    private const int MEMORY_COUNT = 20;
    private List<string> _memory = new List<string>();
    private int _memoryIndex = 0;
    private bool _inMemory = false;

    private TD.Map _map;

    private void Start() {
        Bind();
        _inputField.text = "";
        _inputField.ActivateInputField();
        _inputField.onEndEdit.AddListener(delegate { OnEndEdit(); });
        DisplayMessage(Utils.LoadTextFromFile("greeting.txt"));
    }

    private void Update() {
        //TODO: holding?
        if(Input.GetKeyDown(KeyCode.UpArrow)) {
            BrowseMemory(1);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            BrowseMemory(-1);
        }

    }

    private void BrowseMemory(int step) {
        if(!_inMemory) {
            _inMemory = true;
            _memoryIndex = -1;
        }
        _memoryIndex += step;
        if(_memoryIndex < 0) {
            _inMemory = false;
            _inputField.text = "";
            return;
        }
        if(_memoryIndex < _memory.Count)
            _inputField.text = _memory[_memoryIndex];
        else
            _memoryIndex -= step;
        _inputField.caretPosition = _inputField.text.Length;
    }

    #region commands
    private void Bind() {
        if(_isInit)
            return;
        _commands = new Dictionary<string, Action<string>>();
        _commands.Add("-clear", Clear);
        _commands.Add("-help", Help);
        _commands.Add("start", GameStart);
        _commands.Add("build", Build);
    }

    private void Build(string obj) {
        if(_map == null)
            throw new InvalidOperationException("Not in game mode");

        string[] args = obj.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

        if(args.Length < 4)
            throw new ArgumentException("Not enough parameters specified");
        int x = 0;
        string xString = args[2];

        int y = 0;
        string yString = args[2];

        if(!int.TryParse(args[2], out x))
            throw new ArgumentException("Couldn't read x positon");
        if(!int.TryParse(args[3], out y))
            throw new ArgumentException("Couldn't read y positon");
        Turret.Build(args[1], _map.GetTile(x, y));
    }

    private void GameStart(string obj) {
        Utils.LoadTextFromFileNonBlocking("Map1.txt",GenerateMap);
    }

    private void GenerateMap(string obj) {
        _map = TD.Map.Generate(obj);
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
        _inMemory = false;
        _inputField.text = "";
        _inputField.ActivateInputField();
        if(string.IsNullOrEmpty(cmd))
            return;
        Memorize(cmd);
        Push(cmd);
        Interpret(cmd);
    }

    private void Memorize(string cmd) {
        if(_memory.Count > 0 && _memory[0] == cmd)
            return;
        _memory.Insert(0,cmd);
        while(_memory.Count > MEMORY_COUNT) {
            _memory.RemoveAt(_memory.Count - 1);
        }
    }

    private void Push(string message, bool isError = false) {
        Text current = _line;
        if(_lines.Count < _maxLinesCount) {
            current = Instantiate(_line);
            current.transform.SetParent(_content);
            current.transform.localScale = Vector3.one;
        } else {
            current = _lines.Dequeue();
        }
        _lines.Enqueue(current);
        current.transform.SetAsLastSibling();
        current.text = message;
        current.color = isError ? _errorColor : _regularColor;
    }

    //TODO: no idea what to do here
    private void Interpret(string cmd) {
        bool found = false;
        foreach(var command in _commands) {
            if(cmd.StartsWith(command.Key)) {
                try {
                    command.Value(cmd);
                } catch(Exception e) {
                    Push(e.Message, true);
                    Debug.Log(e.Message);
                }
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
    
