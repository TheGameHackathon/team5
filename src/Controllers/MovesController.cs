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
            VectorDto deltaPos = null;
            switch (userInput.KeyPressed)
            {
                case 37:
                    deltaPos = new VectorDto(-1, 0);
                     break;
                case 38:
                    deltaPos = new VectorDto(0, -1);
                    break;
                case 39:
                    deltaPos = new VectorDto(1, 0);
                    break;
                case 40:
                    deltaPos = new VectorDto(0, 1);
                    break;
            }
            var game = GamesRepo.AGameDto(new VectorDto(1, 1));
            // if (userInput.ClickedPos != null)
            //     game.Cells.First(c => c.Type == "color4").Pos = userInput.ClickedPos;
            return Ok(game);
        }
    }
}