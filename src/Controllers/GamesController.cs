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
            var map1_l1 = System.IO.File.ReadAllText("map1_l1.txt");
            var map1_l2 = System.IO.File.ReadAllText("map1_l2.txt");
            
            GamesRepo.CreateGame(new []{map1_l1, map1_l2});
            return Ok(GamesRepo.SetNewVectorFor("User", new VectorDto(1, 1)));
        }
    }
    
    
}
    