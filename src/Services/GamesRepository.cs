using System;
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

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    cells[j + i * height] = new CellDto((j + i * height).ToString(), new VectorDto(i, j), "b", "", 0);
                }
            }
            
            for (var i = 1; i < width - 1; i++)
            {
                for (var j = 1; j < height - 1; j++)
                {
                    cells[j + i * height] = new CellDto((j + i * height).ToString() + "sdsd", new VectorDto(i, j), "b2", "", 2);
                }
            }
            
            cells[^1] = new CellDto("User", new VectorDto(5, 5), "u", "", 2);
            cells[^2] = new CellDto("Point", new VectorDto(6, 6), "b1", "*", 1);
            
            return CurrentGame = new GameDto(null, cells, true, true, width, height, Guid.Empty, false, 0);
        }

        public static bool IsEmptyForObject(string objTag, VectorDto position)
        {
           return CurrentGame.Cells
               .Where(x => x.Pos.Equals(position))
               .FirstOrDefault(x => x.ZIndex == CurrentGame.Cells[FindIndexByTag(objTag)].ZIndex) is null;
        }

        public static bool IsFinished()
        {
            throw new NotImplementedException();
        }

        public static bool TryPushObject(string pusherTag, VectorDto pusherDelta)
        {
            throw new NotImplementedException();
        }

        public static GameDto MoveObjOnDelta(string objTag, VectorDto delta)
        {
            var index = FindIndexByTag(objTag);
            var objPos = CurrentGame.Cells[index].Pos;
            var movedVector = new VectorDto(delta.X + objPos.X, delta.Y + objPos.Y);
            
            return SetNewVectorFor(objTag, movedVector);
        }

        public static GameDto SetNewVectorFor(string objTag, VectorDto to)
        {
            if (IsEmptyForObject(objTag, to)) 
                return CurrentGame;
            
            var index = FindIndexByTag(objTag);
            var obj = CurrentGame.Cells[index];
            CurrentGame.Cells[index] = new CellDto(objTag, to, obj.Type, obj.Content, obj.ZIndex);

            return GetGame();
        }

        private static int FindIndexByTag(string tag)
        {
            return CurrentGame.Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == tag).i;
        }

        private static GameDto GetGame()
        {
            return CurrentGame = new GameDto(null, CurrentGame.Cells, true, true, CurrentGame.Width, CurrentGame.Height, CurrentGame.Id, false, 0);
        }
    }
}