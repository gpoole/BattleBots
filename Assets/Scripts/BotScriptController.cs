using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

class InvalidCommandException : Exception
{
    public InvalidCommandException(string commandName) : base("Command " + commandName + " doesn't exist.") {}
}

class InvalidArgumentsException : Exception
{
    
}

// class Formula
// {
//     private string _formula;
//     
//     public Formula(string formula)
//     {
//         _formula = formula;
//     }
//
//     public float Evaluate(BotScriptContext context)
//     {
//         
//     }
// }

abstract class ScriptCommand
{
    public static ScriptCommand Parse(string line)
    {
        string[] parts = line.Split(' ');
        var commandName = parts[0];
        var arguments = parts[1..];
        switch (commandName)
        {
            case "move":
                return new MoveCommand(arguments);
            case "turn":
                return new TurnCommand(arguments);
            default:
                throw new InvalidCommandException(commandName);
        }
    }

    public abstract UniTask Execute(BotScriptContext context);
}

class MoveCommand : ScriptCommand
{
    private float _distance;

    public MoveCommand(string[] arguments)
    {
        // FIXME: check validity
        _distance = float.Parse(arguments[0]);
    }

    public override async UniTask Execute(BotScriptContext context)
    {
        await context.TargetBot.MoveRoutine(_distance).ToUniTask(context.TargetBot);
    }

    public override string ToString()
    {
        return $"move {_distance}";
    }
}

class TurnCommand : ScriptCommand
{
    private float _angle;

    public TurnCommand(string[] arguments)
    {
        // FIXME: check validity
        _angle = float.Parse(arguments[0]);
    }
    
    public override async UniTask Execute(BotScriptContext context)
    {
        await context.TargetBot.TurnRoutine(_angle).ToUniTask(context.TargetBot);
    }

    public override string ToString()
    {
        return $"turn {_angle}";
    }
}

class BotScriptContext
{
    public Bot TargetBot;

    public int CurrentCommand = 0;

    public Dictionary<string, float> Variables;
}

class BotScript
{
    private string _source;

    private List<ScriptCommand> _commands = new();

    private int _currentCommand;

    public BotScript(string source)
    {
        _source = source;
        Parse();
    }

    private void Parse()
    {
        var lines = _source.Split("\n");

        foreach (var line in lines)
        {
            _commands.Add(ScriptCommand.Parse(line));
        }
    }

    public async UniTask Execute(BotScriptContext context)
    {
        Debug.Log("Starting script.");
        foreach (var command in _commands)
        {
            Debug.Log($"{_currentCommand}: ${command}");
            await command.Execute(context);
            _currentCommand++;
        }

        Debug.Log("Completed.");
    }
}

public class BotController : MonoBehaviour
{
    [SerializeField]
    private Bot _bot;

    [SerializeField]
    [TextArea(50,10000)]
    private string _botScript;

    void Start() {
        RunScript();
    }

    private void RunScript()
    {
        var context = new BotScriptContext { TargetBot = _bot };
        var script = new BotScript(_botScript);
        script.Execute(context).Forget();
    }
}
