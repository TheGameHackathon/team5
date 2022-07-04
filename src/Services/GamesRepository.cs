using System;
using System.Collections.Generic;

namespace thegame.Services;

public class GamesRepository
{
    public Dictionary<Guid, FloodFillGame> _activegames = new Dictionary<Guid, FloodFillGame>();
    
}