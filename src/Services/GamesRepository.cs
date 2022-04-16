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
            var height = map[0].Count(x => x == '\n');
            var width = map[0].Length / (height + 1);
            var cells = ParseMap(map);
            return CurrentGame = new GameDto(null, cells, true, true, height, width, Guid.Empty, false, 0);
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
        
        public static GameDto TryPushObject(string pusherTag, VectorDto pusherDelta, out bool isSuccess)
        {
            var pusherId = CurrentGame.Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == pusherTag).i;
            var newPos = new VectorDto(pusherDelta.X + CurrentGame.Cells[pusherId].Pos.X,
                pusherDelta.Y + CurrentGame.Cells[pusherId].Pos.Y);

            if (pusherTag == "User")
                return TryPushUser(newPos, CurrentGame.Cells[pusherId], pusherDelta, out isSuccess);
            isSuccess = false;
            return CurrentGame;
        }

        private static GameDto TryPushUser(VectorDto newUserPos, CellDto userDto, VectorDto delta, out bool isSuccess)
        {
            var objOnNewPos = CurrentGame
                .Cells
                .Where(x => x.Pos.Equals(newUserPos))
                .ToArray();
            if (objOnNewPos.All(x => !IsSolid(x.Type)))
            {
                userDto.Pos += delta;
                isSuccess = true;
                return CurrentGame;
            }

            var box = objOnNewPos.FirstOrDefault(x => x.Type is "box"); 
            if (box != null)
            {
                var objectBehindBoxPos = box.Pos + delta;
                var objectsBehindBox = CurrentGame.Cells
                    .Where(x => x.Pos.Equals(objectBehindBoxPos))
                    .ToArray();
                foreach (var objectBehindBox in objectsBehindBox)
                {
                    if (objectBehindBox.Type is "box" or "wall")
                    {
                        isSuccess = false;
                        return CurrentGame;
                    }
                }

                isSuccess = true;
                userDto.Pos = newUserPos;
                box.Pos = objectBehindBoxPos;
                return CurrentGame;
            }

            isSuccess = false;
            return CurrentGame;
        }

        private static bool IsSolid(string type)
            => type is "box" or "wall";

        public static GameDto MovePlayerOnDelta(string objTag, VectorDto delta)
        {
            Console.WriteLine($"Хочу двинуть на {delta}");
            var index = CurrentGame.Cells.Select((x, i) => (x, i)).FirstOrDefault(x => x.x.Id == objTag).i;
            var movedVector = new VectorDto(delta.X + CurrentGame.Cells[index].Pos.X,
                delta.Y + CurrentGame.Cells[index].Pos.Y);
            var newGame = TryPushObject("User", delta, out var isPushed);
            Console.WriteLine($"Удалось ли сдвинуть = {isPushed}");
            return isPushed ? newGame : CurrentGame;
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
            return CurrentGame = new GameDto(null, CurrentGame.Cells, true, true, CurrentGame.Width, CurrentGame.Height, CurrentGame.Id, IsFinished(), 0);
        }
    }
}