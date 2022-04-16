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

        public static CellDto[] ParseMap(string[] mapLayouts)
        {
            var cells = new List<CellDto>();
            
            foreach (var mapLayout in mapLayouts)
            {
                var map = mapLayout.Split("\n");
                
                for (int i = 0; i < map.Length; i++)
                {
                    var line = map[i];
                    
                    for (int j = 0; j < line.Length; j++)
                    {
                        switch (line[j])
                        {
                            case ' ':
                                cells.Add(new CellDto(Guid.NewGuid().ToString() + ' ', new VectorDto(j, i), "floor", "", -999));
                                break;
                            case '#':
                                cells.Add(new CellDto(Guid.NewGuid().ToString() + '#', new VectorDto(j, i), "wall", "", 3));
                                break;
                            case '*':
                                cells.Add(new CellDto(Guid.NewGuid().ToString() + '*', new VectorDto(j, i), "point", "*", 2));
                                break;
                            case '@':
                                cells.Add(new CellDto(Guid.NewGuid().ToString() + '@', new VectorDto(j, i), "box", "", 3));
                                break;
                            case '&':
                                cells.Add(new CellDto("User", new VectorDto(j, i), "user", "", 3));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return cells.ToArray();
        }

        public static GameDto CreateGame(string[] map)
        {
            var width = 8;
            var height = 9;
            var cells = ParseMap(map);
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
            var cells = CurrentGame.Cells;
            var boxes = cells.Where(x => x.Type == "box").Select(x => (x.Pos.X, x.Pos.Y)).ToHashSet();
            var points = cells.Where(x => x.Type == "point").Select(x => (x.Pos.X, x.Pos.Y)).ToHashSet();

            return points.SetEquals(boxes);
        }

        private static GameDto TryPush(string objTag, VectorDto delta)
        {
            var user = CurrentGame.Cells.First(x => x.Id == objTag);
            var pushedObj = CurrentGame.Cells.FirstOrDefault(
                x => x.ZIndex == user.ZIndex 
                     && x.Type == "box"
                     && x.Pos.Equals(new VectorDto(delta.X + user.Pos.X, delta.Y + user.Pos.Y)));
            var newPushedObjPos = new VectorDto(delta.X * 2 + user.Pos.X, delta.Y * 2 + user.Pos.Y);

            if (pushedObj is null || !IsEmptyForObject(pushedObj.Id, newPushedObjPos)) return CurrentGame;
            SetNewVectorFor(pushedObj.Id, newPushedObjPos);
            return CurrentGame;
        }

        public static GameDto MoveObjOnDelta(string objTag, VectorDto delta)
        {
            var index = FindIndexByTag(objTag);
            var objPos = CurrentGame.Cells[index].Pos;
            var movedVector = new VectorDto(delta.X + objPos.X, delta.Y + objPos.Y);
            
            if (IsEmptyForObject(objTag, movedVector))
               return  SetNewVectorFor(objTag, movedVector);
            TryPush(objTag, delta);
            return SetNewVectorFor(objTag, movedVector);
        }

        public static GameDto SetNewVectorFor(string objTag, VectorDto to)
        {
            if (!IsEmptyForObject(objTag, to)) 
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
            return CurrentGame = new GameDto(null!, CurrentGame.Cells, true, true, CurrentGame.Width, CurrentGame.Height, CurrentGame.Id, IsFinished(), CurrentGame.Score);
        }
    }
}