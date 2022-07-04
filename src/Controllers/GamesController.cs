using Microsoft.AspNetCore.Mvc;
using System;
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
        _repo.StartNewGame(Guid.NewGuid());
        return Ok(TestData.AGameDto(new VectorDto {X = 1, Y = 1}));
    }
}