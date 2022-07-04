using System;

namespace thegame.Services;

public interface IGamesRepository
{
    FloodFillGame StartNewGame(Guid id);
    FloodFillGame GetGame(Guid id);

    void Delete(Guid id);
}