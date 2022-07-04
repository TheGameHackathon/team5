using Microsoft.AspNetCore.Mvc;
using thegame.Models;
using thegame.Services;

namespace thegame.Controllers;

[Route("api/games")]
public class GamesController : Controller
{
    private readonly IGamesRepository _repo;

    public GamesController(IGamesRepository repo)
    {
        _repo = repo;
    }
    [HttpPost]
    public IActionResult Index()
    {
        return Ok(new MapGenerator().StartNewGame(MapGenerator.Difficult.Medium));
    }
}