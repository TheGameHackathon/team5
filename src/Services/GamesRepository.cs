using System;
using System.Collections.Generic;
using thegame.Models;

namespace thegame.Services;

public class GamesRepository : IGamesRepository
{
    private readonly IFieldGenerator _fieldGenerator;
    private Dictionary<Guid, FloodFillGame> _activegames = new Dictionary<Guid, FloodFillGame>();
    public GamesRepository(IFieldGenerator fieldGenerator)
    {
        _fieldGenerator = fieldGenerator;
    }

    public FloodFillGame GetGame(Guid id)
    {
        return _activegames[id];
    }

    public void Delete(Guid id)
    {
        _activegames.Remove(id);
    }

    public void AddNewGame(GameDto gameDto)
    {
        var game = new FloodFillGame(gameDto.Cells, 
            gameDto.Width, gameDto.Height, gameDto.Id, 
            gameDto.IsFinished, gameDto.Score);
        _activegames[gameDto.Id] = game;
    }
}