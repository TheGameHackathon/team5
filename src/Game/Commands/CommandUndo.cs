using thegame.Game;

namespace thegame.Commands;

public class CommandUndo : ICommand
{
    public string name { get; private set;}
   

    public CommandUndo()
    {
        name = "Undo";
    }
    public override void Apply(FloodFillGame game, UserInput input)
    {
        throw new System.NotImplementedException();
    }
}