using Microsoft.AspNetCore.Mvc;
using thegame.Models;
using thegame.Services;

namespace thegame.Controllers;

[Route("api/games")]
public class GamesController : Controller
{
    private readonly IGamesRepository _repo;
    private readonly IFieldGenerator _fieldGenerator;

    public GamesController(IGamesRepository repo, IFieldGenerator fieldGenerator)
    {
        _repo = repo;
        _fieldGenerator = fieldGenerator;
    }
    [HttpPost]
    public IActionResult Index()
    {
        var gameDto = _fieldGenerator.GenerateNewField(0);

        _repo.AddNewGame(gameDto);
        return Ok(gameDto);
        //var field = _fieldGenerator.sta
        //return Ok(new FieldGenerator().StartNewGame(FieldGenerator.Difficult.Medium));
    }
}