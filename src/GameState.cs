using System.Collections;
using System.Collections.Generic;
using System.Linq;
using thegame.Models;

namespace thegame
{
    public class GameState
    {
        private GameDto _game;
        public GameState(GameDto game)
        {
            _game = game;
        }

        public void Step(string color)
        {
            var queue = new Queue<CellDto>();
            queue.Enqueue(_game.Cells[0]);
            var baseColor = _game.Cells[0].Type;
            _game.Cells[0].Type = color;
            var used = new HashSet<VectorDto>();
            var neignbours = new List<VectorDto>();

            while (TryGetNeighbours(queue.Peek(), neignbours, baseColor))
            {
                var node = queue.Dequeue();
                foreach(var neighbour in neignbours.Where(x => !used.Contains(x)))
                {
                    queue.Enqueue(_game.Cells[neighbour.X + neighbour.Y * _game.Width]);
                }
                neignbours.Clear();
                node.Type = color;
                used.Add(new VectorDto() { X = node.Pos.X, Y = node.Pos.Y });
            }
        }

        public bool TryGetNeighbours(CellDto cell, List<VectorDto> neigbours, string color)
        {
            var flag = false;
            if (cell.Pos.Y + _game.Width <= _game.Cells.Length &&
                _game.Cells[cell.Pos.X + cell.Pos.Y * _game.Width + _game.Width].Type == color)
            {
                neigbours.Add(new VectorDto() { X = cell.Pos.X, Y = cell.Pos.Y + 1});
                flag = true;
            }    
                
            if (cell.Pos.Y - _game.Width >= 0 &&
                _game.Cells[cell.Pos.X + cell.Pos.Y * _game.Width - _game.Width].Type == color)
            {
                neigbours.Add(new VectorDto() { X = cell.Pos.X, Y = cell.Pos.Y - 1});
                flag = true;
            }
                
            if (cell.Pos.X + 1 <= _game.Cells.Length &&
                _game.Cells[cell.Pos.X + 1 + cell.Pos.Y *_game.Width].Type == color)
            {
                neigbours.Add(new VectorDto() { X = cell.Pos.X + 1, Y = cell.Pos.Y });
                flag = true;
            }
                
            if (cell.Pos.X - 1 >= _game.Cells.Length &&
                _game.Cells[cell.Pos.X - 1 + cell.Pos.Y * _game.Width + _game.Width].Type == color)
            {
                neigbours.Add(new VectorDto() { X = cell.Pos.X - 1, Y = cell.Pos.Y });
                flag = true;
            }

            return flag;
        }
    }
}
