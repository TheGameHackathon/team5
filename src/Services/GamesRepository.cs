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
            // var cells = new CellDto[width * height + 2];
            //
            // for (var i = 0; i < width; i++)
            // {
            //     for (var j = 0; j < height; j++)
            //     {
            //         cells[j + i * height] = new CellDto((j + i * height).ToString(), new VectorDto(i, j), "b", "", 0);
            //     }
            // }
            //
            // for (var i = 1; i < width - 1; i++)
            // {
            //     for (var j = 1; j < height - 1; j++)
            //     {
            //         cells[j + i * height] = new CellDto((j + i * height).ToString() + "sdsd", new VectorDto(i, j), "b2", "", 2);
            //     }
            // }
            //
            // cells[^1] = new CellDto("User", new VectorDto(5, 5), "u", "", 2);
            // cells[^2] = new CellDto("Point", new VectorDto(6, 6), "b1", "*", 1);
            
            // cells[^1] = new CellDto("User", new VectorDto(5, 5), "u", "", 0);
            // cells[^2] = new CellDto("Point", new VectorDto(6, 6), "b2", "*", 1);
            // cells[^3] = new CellDto("Box", new VectorDto(5, 6), "b2", "[  ]", 2);
            
            return CurrentGame = new GameDto(null, cells, true, true, width, height, Guid.Empty, IsFinished(cells), 0);
        }

        public static bool IsEmptyForObject(string objTag, VectorDto position)
        {
           return CurrentGame.Cells
               .Where(x => x.Pos.Equals(position))
               .FirstOrDefault(x => x.ZIndex == CurrentGame.Cells[FindIndexByTag(objTag)].ZIndex) is null;
        }

        public static bool IsFinished(CellDto[] cells)
        {
            var boxes = cells.Where(x => x.Id == "Box").Select(x => (x.Pos.X, x.Pos.Y)).ToHashSet();
            var points = cells.Where(x => x.Id == "Point").Select(x => (x.Pos.X, x.Pos.Y)).ToHashSet();
            return points.SetEquals(boxes);
        }

        public bool TryPushObject(string pusherTag, VectorDto pusherDelta)
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