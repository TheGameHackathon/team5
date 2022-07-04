using System;

namespace thegame.Services;

public interface IGameRepository
{
    FloodFillGame StartNewGame(Guid id);
    FloodFillGame GetGame(Guid id);

    void Delete(Guid id);
}