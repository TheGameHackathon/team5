using System;


namespace thegame;

public class FloodFillGame
{
    public FloodFillGame(Cell[] cells, int width, int height, Guid id, bool isFinished, int score)
    {
        Cells = cells;

        Width = width;
        Height = height;
        Id = id;
        IsFinished = isFinished;
        Score = score;
    }

    public Cell[] Cells { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Guid Id { get; set; }
    public bool IsFinished { get; set; }
    public int Score { get; set; }
}