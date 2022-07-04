using System;
using thegame.Services;
using System.Collections.Generic;
using System.Linq;
using thegame.Commands;
using thegame.Game;


namespace thegame;

public class FloodFillGame
{
    
    public List<ICommand> commands = new List<ICommand>();
    
    public FloodFillGame(Field field, int width, int height, Guid id, bool isFinished, int score)
    {
        Field = field;
        Width = width;
        Height = height;
        Id = id;
        IsFinished = isFinished;
        Score = score;
        
        commands.Add(new CommandPickColor());
    }

    public Field Field { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Guid Id { get; set; }
    public bool IsFinished { get; set; }
    public int Score { get; set; }

    public void executeCommand(string command, UserInput input)
    {
        commands.FirstOrDefault(x=> x.name == command).Apply(this,input);
    }
}