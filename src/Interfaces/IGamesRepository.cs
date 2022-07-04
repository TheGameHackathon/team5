using System;
using thegame.Models;

namespace thegame.Services;

public interface IGamesRepository
{
    FloodFillGame StartNewGame(Guid id);
    void AddNewGame(GameDto gameDto);
    FloodFillGame GetGame(Guid id);

    void Delete(Guid id);
}