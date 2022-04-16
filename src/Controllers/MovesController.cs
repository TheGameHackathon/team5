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
            if (userInput.ClickedPos is not null)
                Ok(GamesRepo.SetNewVectorFor("User", userInput.ClickedPos));
            return Ok(GamesRepo.CurrentGame);
        }
    }
}