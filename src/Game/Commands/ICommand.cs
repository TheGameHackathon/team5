using thegame.Game;

namespace thegame.Commands;

public abstract class ICommand
{
    public string name { get; private set;}
    public abstract void Apply(FloodFillGame game,UserInput input);
}