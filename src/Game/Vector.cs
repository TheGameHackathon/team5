namespace thegame;

public class Vector
{
    public int X { get; set; }
    public int Y { get; set; }
    public override bool Equals(object? obj)
    {
        var vector = (Vector)obj;
        return X == vector.X && vector.Y == Y;
    }

    public override int GetHashCode()
    {
        return X * 89 + Y;
    }
}