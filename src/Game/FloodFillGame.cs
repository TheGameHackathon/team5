using System;
using thegame.Services;
using System.Collections.Generic;
using thegame.Commands;


namespace thegame;

public class FloodFillGame
{
    
    public List<ICommand> commands = new List<ICommand>();
    
    public FloodFillGame(Guid id, Field field, int width, int height, bool isFinished=false, int score=0)
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

    
}