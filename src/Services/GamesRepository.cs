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

    public FloodFillGame StartNewGame(Guid id)
    {
        throw new NotImplementedException();
    }

    public FloodFillGame GetGame(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public void AddNewGame(GameDto gameDto)
    {
        var game = new FloodFillGame(gameDto.Cells, 
            gameDto.Width, gameDto.Height, gameDto.Id, 
            gameDto.IsFinished, gameDto.Score);
        _activegames.Add(gameDto.Id, game);
    }
}