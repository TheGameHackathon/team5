using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using thegame.Models;

namespace thegame.Services
{
    public class GamesRepo
    {
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

        public static GameDto CreateGame(Guid id, string[] map)
        {
            var width = 8;
            var height = 9;
            var cells = ParseMap(map);
            Games[id] = new GameDto(null, cells, true, true, width, height, id, false, 0);
            return Games[id];
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
        
        public static GameDto TryPushObject(Guid id, string pusherTag, VectorDto pusherDelta, out bool isSuccess)
        {
            var pusherId = Games[id].Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == pusherTag).i;
            var newPos = new VectorDto(pusherDelta.X + Games[id].Cells[pusherId].Pos.X,
                pusherDelta.Y + Games[id].Cells[pusherId].Pos.Y);

            if (pusherTag == "User")
                return TryPushUser(id, newPos, Games[id].Cells[pusherId], pusherDelta, out isSuccess);
            isSuccess = false;
            return Games[id];
        }

        private static GameDto TryPushUser(Guid id, VectorDto newUserPos, CellDto userDto, VectorDto delta, out bool isSuccess)
        {
            var objOnNewPos = Games[id]
                .Cells
                .Where(x => x.Pos.Equals(newUserPos))
                .ToArray();
            if (objOnNewPos.All(x => !IsSolid(x.Type)))
            {
                userDto.Pos += delta;
                isSuccess = true;
                return Games[id] = GetGame(id);
            }

            var box = objOnNewPos.FirstOrDefault(x => x.Type is "box"); 
            if (box != null)
            {
                var objectBehindBoxPos = box.Pos + delta;
                var objectsBehindBox = Games[id].Cells
                    .Where(x => x.Pos.Equals(objectBehindBoxPos))
                    .ToArray();
                foreach (var objectBehindBox in objectsBehindBox)
                {
                    if (objectBehindBox.Type is "box" or "wall")
                    {
                        isSuccess = false;
                        return Games[id] = GetGame(id);
                    }
                }

                isSuccess = true;
                userDto.Pos = newUserPos;
                box.Pos = objectBehindBoxPos;
                return Games[id] = GetGame(id);
            }

            isSuccess = false;
            return Games[id] = GetGame(id);
        }

        private static bool IsSolid(string type)
            => type is "box" or "wall";

        public static GameDto MovePlayerOnDelta(Guid id, string objTag, VectorDto delta)
        {
            Console.WriteLine($"Хочу двинуть на {delta}");
            var index = Games[id].Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == objTag).i;
            var movedVector = new VectorDto(delta.X + Games[id].Cells[index].Pos.X,
                delta.Y + Games[id].Cells[index].Pos.Y);
            var newGame = TryPushObject(id, "User", delta, out var isPushed);
            Console.WriteLine($"Удалось ли сдвинуть = {isPushed}");
            return isPushed ? newGame : Games[id];
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
            return new GameDto(null!, Games[id].Cells, true, true, Games[id].Width, Games[id].Height, Games[id].Id, IsFinished(id), 0);
        }
    }
}