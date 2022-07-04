using System;

namespace thegame.Services;

public interface IGamesRepository
{
    Filed StartNewGame(Guid id);
    Filed GetGame(Guid id);

    void Delete(Guid id);
}