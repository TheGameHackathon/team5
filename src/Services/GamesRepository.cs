using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using thegame.Models;

namespace thegame.Services
{
    public class GamesRepo
    {
        public static GameDto CurrentGame { get; private set; }

        public static GameDto CreateGame()
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
            
            cells[^1] = new CellDto("User", new VectorDto(5, 5), "u", "", 0);
            
            return CurrentGame = new GameDto(cells, true, true, width, height, Guid.Empty, false, 0);
        }

        public static GameDto SetNewVectorFor(string objTag, VectorDto to)
        {
            var index = CurrentGame.Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == objTag).i;
            
            CurrentGame.Cells[index] = new CellDto(objTag, to, CurrentGame.Cells[index].Type, CurrentGame.Cells[index].Content, CurrentGame.Cells[index].ZIndex);
            
            return CurrentGame = new GameDto(CurrentGame.Cells, true, true, CurrentGame.Width, CurrentGame.Height, CurrentGame.Id, false, 0);
        }
    }
}