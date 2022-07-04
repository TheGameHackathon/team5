using System;
using thegame.Models;

namespace thegame.Services
{
    public class FieldGenerator : IFieldGenerator
    {
        public GameDto GenerateNewField(Difficult diff)
        {
            var (width, height, colorAmount) = GetParametersForField(diff);

            var testCells = new CellDto[width * height];

            for (var j = 0; j < width; j++)
            {
                for (var i = 0; i < height; i++)
                {
                    var index = j * width + i;

                    var colorIndex = new Random().Next(1, colorAmount + 1);
                    var color = $"color{colorIndex}";

                    var cell = new CellDto(index.ToString(),
                                            new VectorDto { X = i, Y = j }, color, "", 0);

                    testCells[index] = cell;
                }
            }
            return new GameDto(testCells, true, true, width, height, Guid.NewGuid(), false, 0);
        }

        private (int, int, int) GetParametersForField(Difficult dif)
        {
            var width = 8;
            var height = 8;
            var colorAmount = 3;

            switch (dif)
            {
                case Difficult.Eazy:
                    width = height = 8;
                    colorAmount = 3;
                    break;
                case Difficult.Medium:
                    width = height = 21;
                    colorAmount = 5;
                    break;
                case Difficult.Hard:
                    width = height = 55;
                    colorAmount = 8;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Нахера ты мне передал эту сложность{dif}");
            }
            return (width, height, colorAmount);
        }


        public enum Difficult
        {
            Eazy,
            Medium,
            Hard
        }
    }
}
