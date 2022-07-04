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
    public Stack<CellDto[]> history = new Stack<CellDto[]>();
    
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
        var queue = new Stack<CellDto>();
        queue.Push(Field[0]);

        var baseColor = Field[0].Type;
        if (color == baseColor)
            return IsFinished;
        Score += 1;
        Field[0].Type = color;

        var used = new HashSet<Vector>();
        while (queue.Count > 0)
        {
            var node = queue.Pop();
            var neigbhours = TryGetNeighbours(node, baseColor);
            foreach (var neighbour in neigbhours.Where(x => !used.Contains(x)))
            {
                queue.Push(Field[neighbour.X + neighbour.Y * Width]);
            }

            node.Type = color;
            used.Add(new Vector() { X = node.Pos.X, Y = node.Pos.Y });
        }
        return Field.All(cell => cell.Type == color);
    }

    public bool StepAI()
    {
        var colorCount = 3;
        var queue = new Stack<CellDto>();
        queue.Push(Field[0]);
        var baseColor = Field[0].Type;
        var printedCellsCount = new Dictionary<int, int>();
        var colors = new Dictionary<int, string>();
        for (var i = 1; i <= colorCount; i++)
            colors.Add(i, "color" + i.ToString());

        for (var i = 1; i <= colorCount; i++)
        {
            if (colors[i] == baseColor) continue;
            var currField = new CellDto[Field.Length];
            var currStack = new Stack<CellDto>();
            for (var j = 0; j < currField.Length; j++)
                currField[j] = Field[j];
            currField[0].Type = colors[i];
            var currUsed = new HashSet<Vector>();
            var currNeignbours = new List<Vector>();

            while (currStack.Count > 0)
            {
                var node = currStack.Pop();
                var neignbours = TryGetNeighbours(node, baseColor);

                foreach (var neighbour in neignbours.Where(x => !currUsed.Contains(x)))
                    currStack.Push(Field[neighbour.X + neighbour.Y * Width]);

                node.Type = colors[i];
                currUsed.Add(new Vector() { X = node.Pos.X, Y = node.Pos.Y });
            }
            printedCellsCount.Add(i, currUsed.Count);
        }
        var bestColor = string.Empty;
        var maxPrintedCells = int.MinValue;
        foreach (var cell in printedCellsCount)
            if (cell.Value > maxPrintedCells) bestColor = colors[cell.Key];
        Field[0].Type = bestColor;
        queue.Push(Field[0]);

        Score += 1;

        var used = new HashSet<Vector>();
        while (queue.Count > 0)
        {
            var node = queue.Pop();
            var neignbours = TryGetNeighbours(node, baseColor);

            foreach (var neighbour in neignbours.Where(x => !used.Contains(x)))
            {
                queue.Push(Field[neighbour.X + neighbour.Y * Width]);
            }

            node.Type = bestColor;
            used.Add(new Vector() { X = node.Pos.X, Y = node.Pos.Y });
        }
        return Field.All(cell => cell.Type == bestColor);
    }

    public List<Vector> TryGetNeighbours(CellDto cell, string color)
    {
        var result = new List<Vector>();

        if (cell.Pos.Y != Height &&
            cell.Pos.X + cell.Pos.Y * Width + Width < Field.Length &&
            Field[cell.Pos.X + cell.Pos.Y * Width + Width].Type == color)
        {
            result.Add(new Vector() { X = cell.Pos.X, Y = cell.Pos.Y + 1 });
        }

        if (cell.Pos.Y != 0 &&
            cell.Pos.X + cell.Pos.Y * Width - Width >= 0 &&
            Field[cell.Pos.X + cell.Pos.Y * Width - Width].Type == color)
        {
            result.Add(new Vector() { X = cell.Pos.X, Y = cell.Pos.Y - 1 });
        }

        if (cell.Pos.X != Width &&
            cell.Pos.X + 1 + cell.Pos.Y * Width < Field.Length &&
            Field[cell.Pos.X + 1 + cell.Pos.Y * Width].Type == color)
        {
            result.Add(new Vector() { X = cell.Pos.X + 1, Y = cell.Pos.Y });
        }

        if (cell.Pos.X != 0 &&
            cell.Pos.X - 1 + cell.Pos.Y * Width >= 0 &&
            Field[cell.Pos.X - 1 + cell.Pos.Y * Width].Type == color)
        {
            result.Add(new Vector() { X = cell.Pos.X - 1, Y = cell.Pos.Y });
        }
        return result;
    }

    public void Move(UserInputDto userInput)
    {
        if(userInput.KeyPressed != 0)
        {
            return;
        }
        var color = Field[userInput.ClickedPos.X + userInput.ClickedPos.Y * Width].Type;
        
        List<CellDto> d = new List<CellDto>() { };
        foreach (var e in Field)
        {
            var g = new CellDto(e.Id,e.Pos,e.Content,e.Content,e.ZIndex);
            g.Type = e.Type;
            d.Add(g);
        }
  
        history.Push(d.ToArray());
        IsFinished = ColorStep(color);
    }
    
    public void Undo()
    {
        if(history.Count == 0) return;
        Field = history.Pop();
        
    }
}