using System;

namespace thegame.Services;

public interface IGamesRepository
{
    FloodFillGame StartNewGame(Guid id, int width, int height);
    FloodFillGame GetGame(Guid id);

    void Delete(Guid id);
}