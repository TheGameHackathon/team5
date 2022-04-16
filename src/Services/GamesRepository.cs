using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using thegame.Models;

namespace thegame.Services
{
    public class GamesRepo
    {
        public static GameDto AGameDto(VectorDto movingObjectPosition)
        {
            var width = 8;
            var height = 9;
            var cells = new CellDto[width * height + 1];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cells[j + i * height] = new CellDto((i * j).ToString(), new VectorDto(i, j), "b", "", 0);
                }
            }
            
            for (int i = 1; i < width - 1; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    cells[j + i * height] = new CellDto((i * j).ToString(), new VectorDto(i, j), "b2", "", 0);
                }
            }
            
            cells[cells.Length - 1] = new CellDto((5 * 5).ToString(), new VectorDto(5, 5), "u", "", 0);
            
            return new GameDto(cells, true, true, width, height, Guid.Empty, movingObjectPosition.X == 0, movingObjectPosition.Y);
        }
    }
}