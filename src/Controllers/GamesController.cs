using Microsoft.AspNetCore.Mvc;
using thegame.Models;
using thegame.Services;

namespace thegame.Controllers
{
    [Route("api/games")]
    public class GamesController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            GamesRepo.CreateGame();
            return Ok(GamesRepo.SetNewVectorFor("User", new VectorDto(1, 1)));
        }
    }
}
    