namespace thegame.Commands;

public class CommandPickColor : ICommand
{
    public string name { get; private set;}

    public CommandPickColor()
    {
        name = "PickColor";
    }

    public override void Apply()
    {
        throw new System.NotImplementedException();
    }
}