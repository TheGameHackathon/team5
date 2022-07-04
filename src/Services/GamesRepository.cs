using System;
using System.Collections.Generic;

namespace thegame.Services;

public class GamesRepository : IGamesRepository
{
    private readonly IFieldGenerator _fieldGenerator;
    private Dictionary<Guid, FloodFillGame> _activegames = new Dictionary<Guid, FloodFillGame>();
    public GamesRepository(IFieldGenerator fieldGenerator)
    {
        _fieldGenerator = fieldGenerator;
    }

    public FloodFillGame StartNewGame(Guid id, int width, int height)
    {
        _activegames[id] = new FloodFillGame(id, _fieldGenerator.GenerateNewField(), width, height);
        return _activegames[id];
    }

    public FloodFillGame GetGame(Guid id)
    {
        return _activegames[id];
    }

    public void Delete(Guid id)
    {
        _activegames.Remove(id);
    }
}