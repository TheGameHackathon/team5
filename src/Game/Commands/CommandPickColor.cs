using thegame.Models;

namespace thegame.Commands;

public class CommandPickColor : ICommand
{
    public string name { get; private set;}
    
    public CommandPickColor()
    {
        name = "PickColor";
    }
    
    public override void Apply(UserInput input)
    {
        throw new System.NotImplementedException();
    }

}