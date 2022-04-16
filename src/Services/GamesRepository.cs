using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using thegame.Models;

namespace thegame.Services
{
    public class GamesRepo
    {
        public static string[][] maps =
        {
            new[] {"map2_l1.txt", "map2_l2.txt"},
            new[] {"map1_l1.txt", "map1_l2.txt"},
        };
        
        public static GameDto CurrentGame { get; private set; }

        public static Dictionary<Guid, GameDto> Games = new();
        
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

        public static GameDto CreateGame(Guid guid, int mapId)
        {
            var map = maps[mapId].Select(System.IO.File.ReadAllText).ToArray();
            var height = map[0].Count(x => x == '\n');
            var width = map[0].Length / (height + 1);
            var cells = ParseMap(map);
            return Games[guid] = new GameDto(mapId, null, cells, true, true, height, width, guid, false, 0);
        }

        public static bool IsEmptyForObject(Guid id, string objTag, VectorDto position)
        {
           return Games[id].Cells
               .Where(x => x.Pos.Equals(position))
               .FirstOrDefault(x => x.ZIndex == Games[id].Cells[FindIndexByTag(id, objTag)].ZIndex) is null;
        }

        public static bool IsFinished(Guid id)
        {
            var cells = Games[id].Cells;
            var boxes = cells.Where(x => x.Type == "box").Select(x => (x.Pos.X, x.Pos.Y)).ToHashSet();
            var points = cells.Where(x => x.Type == "point").Select(x => (x.Pos.X, x.Pos.Y)).ToHashSet();
            return points.SetEquals(boxes);
        }

        private static GameDto TryPush(Guid gameId, string objTag, VectorDto delta)
        {
            var currentGame = Games[gameId];
            var user = currentGame.Cells.First(x => x.Id == objTag);
            var pushedObj = currentGame.Cells.FirstOrDefault(
                x => x.ZIndex == user.ZIndex 
                     && x.Type == "box"
                     && x.Pos.Equals(new VectorDto(delta.X + user.Pos.X, delta.Y + user.Pos.Y)));
            var newPushedObjPos = new VectorDto(delta.X * 2 + user.Pos.X, delta.Y * 2 + user.Pos.Y);

            if (pushedObj is null || !IsEmptyForObject(gameId, pushedObj.Id, newPushedObjPos)) return currentGame;
            SetNewVectorFor(gameId, pushedObj.Id, newPushedObjPos);
            return currentGame;
        }

        public static GameDto MoveObjOnDelta(Guid gameId, string objTag, VectorDto delta)
        {
            var index = FindIndexByTag(gameId, objTag);
            var objPos = Games[gameId].Cells[index].Pos;
            var movedVector = new VectorDto(delta.X + objPos.X, delta.Y + objPos.Y);
            
            if (IsEmptyForObject(gameId, objTag, movedVector))
                return  SetNewVectorFor(gameId, objTag, movedVector);
            TryPush(gameId, objTag, delta);
            return SetNewVectorFor(gameId, objTag, movedVector);
        }

        public static GameDto SetNewVectorFor(Guid id, string objTag, VectorDto to)
        {
            if (!IsEmptyForObject(id, objTag, to)) 
                return Games[id];
            
            var index = FindIndexByTag(id, objTag);
            var obj = Games[id].Cells[index];
            Games[id].Cells[index] = new CellDto(objTag, to, obj.Type, obj.Content, obj.ZIndex);

            return GetGame(id);
        }

        private static int FindIndexByTag(Guid id, string tag)
        {
            return Games[id].Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == tag).i;
        }

        private static GameDto GetGame(Guid id)
        {
            var isFinished = IsFinished(id);
            var currentGame = Games[id];
            if (isFinished && !IsLastMap(currentGame))
                return CreateGame(id, currentGame.MapId + 1);
            
            return new GameDto(currentGame.MapId, null, currentGame.Cells, true, true, currentGame.Width, currentGame.Height, currentGame.Id, isFinished, currentGame.Score);
        }

        private static bool IsLastMap(GameDto game)
        {
            return game.MapId + 1 == maps.Length;
        }
    }
}