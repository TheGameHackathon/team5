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
            var cells = new CellDto[width * height + 2];

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
            cells[^2] = new CellDto("Point", new VectorDto(6, 6), "b1", "*", 1);
            
            return CurrentGame = new GameDto(null, cells, true, true, width, height, Guid.Empty, false, 0);
        }

        public bool IsEmptyForObject(string objTag, VectorDto position)
        {
            throw new NotImplementedException();
        }

        public bool IsFinished()
        {
            throw new NotImplementedException();
        }

        public bool TryPushObject(string pusherTag, VectorDto pusherDelta)
        {
            throw new NotImplementedException();
        }

        public static GameDto MoveObjOnDelta(string objTag, VectorDto delta)
        {
            var index = CurrentGame.Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == objTag).i;
            var movedVector = new VectorDto(delta.X + CurrentGame.Cells[index].Pos.X,
                delta.Y + CurrentGame.Cells[index].Pos.Y);

            return SetNewVectorFor(objTag, movedVector);
        }

        public static GameDto SetNewVectorFor(string objTag, VectorDto to)
        {
            var index = CurrentGame.Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == objTag).i;
            
            CurrentGame.Cells[index] = new CellDto(objTag, to, CurrentGame.Cells[index].Type, CurrentGame.Cells[index].Content, CurrentGame.Cells[index].ZIndex);
            
            return CurrentGame = new GameDto(null, CurrentGame.Cells, true, true, CurrentGame.Width, CurrentGame.Height, CurrentGame.Id, false, 0);
        }
    }
}