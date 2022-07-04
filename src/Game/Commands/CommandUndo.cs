namespace thegame.Commands;

public class CommandUndo : ICommand
{
    public string name { get; private set;}
    
    public CommandUndo()
    {
        name = "Undo";
    }
    
    public override void Apply(UserInput input)
    {
        throw new System.NotImplementedException();
    }
}