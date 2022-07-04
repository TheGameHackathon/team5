using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using thegame.Models;
using thegame.Services;

namespace thegame.Controllers;

[Route("api/games/{gameId}/moves")]
public class MovesController : Controller
{
    private readonly IGamesRepository _repo;

    public MovesController(IGamesRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public IActionResult Moves(Guid gameId, [FromBody]UserInputDto userInput)
    {
      
        //var game = TestData.AGameDto(userInput.ClickedPos ?? new VectorDto {X = 1, Y = 1});

        ////_gamesRepository._activegames[gameId].ActComand(usrinput);
        ////маапить игру и отправлять
        
        //if (userInput.ClickedPos != null)
        //    game.Cells.First(c => c.Type == "color4").Pos = userInput.ClickedPos;

        var game =  _repo.GetGame(gameId);
        game.Move(userInput);
        return Ok();
    }
}