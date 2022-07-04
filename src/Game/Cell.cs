namespace thegame;

public class Cell
{
    
    public Cell(string id, Vector pos, string type)
    {
        Id = id;
        Pos = pos;
        Type = type;
    }

    public string Id { get; set; }
    public Vector Pos { get; set; }
    
    public string Type { get; set; }
}