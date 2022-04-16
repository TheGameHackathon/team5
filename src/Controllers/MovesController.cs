using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using thegame.Models;
using thegame.Services;

namespace thegame.Controllers
{
    [Route("api/games/{gameId}/moves")]
    public class MovesController : Controller
    {
        [HttpPost]
        public IActionResult Moves(Guid gameId, [FromBody]UserInputDto userInput)
        {
            VectorDto deltaPos = userInput.KeyPressed switch
            {
                37 => new VectorDto(-1, 0),
                38 => new VectorDto(0, -1),
                39 => new VectorDto(1, 0),
                40 => new VectorDto(0, 1),
                _ => new VectorDto(0, 0)
            };
            var game = GamesRepo.MoveObjOnDelta("User", deltaPos);
            return Ok(game);
        }
    }
}