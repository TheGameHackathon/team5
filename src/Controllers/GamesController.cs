using Microsoft.AspNetCore.Mvc;
using thegame.Models;
using thegame.Services;

namespace thegame.Controllers;

[Route("api/games")]
public class GamesController : Controller
{
    private readonly IGameRepository _repo;

    public GamesController(IGameRepository repo)
    {
        _repo = repo;
    }
    [HttpPost]
    public IActionResult Index()
    {
        return Ok(TestData.AGameDto(new VectorDto {X = 1, Y = 1}));
    }
}