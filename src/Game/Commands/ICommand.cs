namespace thegame.Commands;

public abstract class ICommand
{
    public string name { get; private set;}
    public abstract void Apply(UserInput input);
}