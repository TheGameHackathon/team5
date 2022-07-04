using System;
using System.Collections.Generic;
namespace thegame.Services;

public class GamesRepository : IGamesRepository
{
    private Dictionary<Guid, FloodFillGame> _activegames = new Dictionary<Guid, FloodFillGame>();
    public GamesRepository()
    {
        
    }

    public Filed StartNewGame(Guid id)
    {
        throw new NotImplementedException();
    }

    public Filed GetGame(Guid id)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid id)
    {
        throw new NotImplementedException();
    }

   
}