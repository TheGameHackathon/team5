using System;
using thegame.Services;
using System.Collections.Generic;
using System.Linq;
using thegame.Commands;
using thegame.Models;
using thegame.Game;


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

    public bool ColorStep(string color)
    {
        var queue = new Queue<CellDto>();
        queue.Enqueue(Field[0]);
        var baseColor = Field[0].Type;
        if (color == baseColor)
            return IsFinished;
        Score += 1;
        Field[0].Type = color;
        var used = new HashSet<Vector>();
        var neignbours = new List<Vector>();
        while (queue.Count > 0)
        {
        
            var node = queue.Dequeue();
            TryGetNeighbours(node, neignbours, baseColor);
            foreach(var neighbour in neignbours.Where(x => !used.Contains(x)))
            {
                queue.Enqueue(Field[neighbour.X + neighbour.Y * Width]);
            }
            neignbours.Clear();
            node.Type = color;
            used.Add(new Vector() { X = node.Pos.X, Y = node.Pos.Y });
        }
        return Field.All(cell => cell.Type == color);
    }

        public void TryGetNeighbours(CellDto cell, List<Vector> neigbours, string color)
        {
            if (cell.Pos.X + cell.Pos.Y * Width + Width < Field.Length &&
                Field[cell.Pos.X + cell.Pos.Y * Width + Width].Type == color)
            {
                neigbours.Add(new Vector() { X = cell.Pos.X, Y = cell.Pos.Y + 1});
            }    
                
            if (cell.Pos.X + cell.Pos.Y * Width - Width >= 0 &&
                Field[cell.Pos.X + cell.Pos.Y * Width - Width].Type == color)
            {
                neigbours.Add(new Vector() { X = cell.Pos.X, Y = cell.Pos.Y - 1});
            }
                
            if (cell.Pos.X + 1 + cell.Pos.Y * Width < Field.Length &&
                Field[cell.Pos.X + 1 + cell.Pos.Y * Width].Type == color)
            {
                neigbours.Add(new Vector() { X = cell.Pos.X + 1, Y = cell.Pos.Y });
            }
                
            if (cell.Pos.X - 1 + cell.Pos.Y * Width >= 0 &&
                Field[cell.Pos.X - 1 + cell.Pos.Y * Width].Type == color)
            {
                neigbours.Add(new Vector() { X = cell.Pos.X - 1, Y = cell.Pos.Y });
            }
        }

        public void Move(UserInputDto userInput)
        {
            var color = Field[userInput.ClickedPos.X + userInput.ClickedPos.Y * Width].Type;
            IsFinished = ColorStep(color);
        }
}