using System;
using thegame.Services;
using System.Collections.Generic;
using thegame.Commands;
using thegame.Models;

namespace thegame;

public class FloodFillGame
{
    
    public List<ICommand> commands = new List<ICommand>();

    public FloodFillGame(CellDto[] field, int width, int height, Guid id, bool isFinished, int score)
    {
        Field = field;
        Width = width;
        Height = height;
        Id = id;
        IsFinished = isFinished;
        Score = score;
        
        commands.Add(new CommandPickColor());
    }

    public CellDto[] Field { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Guid Id { get; set; }
    public bool IsFinished { get; set; }
    public int Score { get; set; }

    
}