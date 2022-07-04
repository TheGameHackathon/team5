using System;
using thegame.Models;

namespace thegame.Services;

public class TestData
{
    public static GameDto AGameDto(VectorDto movingObjectPosition)
    {
        var width = 8;
        var height = 8;
        var colorAmount = 3;
        //8*8 3
        //21*21 5
        //55*55 8
        var testCells = new CellDto[width * height];

        for (var j = 0; j < width; j++)
        {
            for (var i = 0; i < height; i++)
            {
                var index = j * width + i;

                var colorIndex = new Random().Next(1, colorAmount+1);
                var color = $"color{colorIndex}";

                var cell = new CellDto(index.ToString(),
                                        new VectorDto { X = i, Y = j }, color, "", 0);
                testCells[index] = cell;
            }
        }

        return new GameDto(testCells, true, true, width, height, Guid.Empty, movingObjectPosition.X == 0, movingObjectPosition.Y);
    }
}