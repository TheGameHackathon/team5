using System;

namespace thegame.Services;

public interface IGameRepository
{
    Filed StartNewGame(Guid id);
    Filed GetGame(Guid id);

    void Delete(Guid id);
}