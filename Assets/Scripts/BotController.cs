using System;
using UnityEngine;

class InvalidCommandException : Exception
{
    public InvalidCommandException(string commandName) : base("Command " + commandName + " doesn't exist.") {}
}

class InvalidArgumentsException : Exception
{
    
}

abstract class BotCommand
{
    public static BotCommand Parse(string line)
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

    public abstract void Run(BotScriptContext context);
}

class MoveCommand : BotCommand
{
    private float _distance;
    
    public MoveCommand(string[] arguments)
    {
        // FIXME: check validity
        _distance = float.Parse(arguments[0]);
    }

    public override void Run(BotScriptContext context)
    {
        context.TargetBot.Move(_distance);
    }
}

class TurnCommand : BotCommand
{
    private float _angle;

    public TurnCommand(string[] arguments)
    {
        // FIXME: check validity
        _angle = float.Parse(arguments[0]);
    }

    public override void Run(BotScriptContext context)
    {
        context.TargetBot.Turn(_angle);
    }
}

class BotScriptContext
{
    public Bot TargetBot;
}

public class BotController : MonoBehaviour
{
    [SerializeField]
    private Bot _bot;

    [SerializeField]
    [TextArea(50,10000)]
    private string _botScript;

    void Start() {
        
    }

    void Update()
    {
        EvaluateBotScript();
    }

    private void EvaluateBotScript()
    {
        var lines = _botScript.Split("\n");
        var context = new BotScriptContext
        {
            TargetBot = _bot
        };

        foreach (var line in lines)
        {
            var nextCommand = BotCommand.Parse(line);
            nextCommand.Run(context);
        }
    }
}
