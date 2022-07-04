using Microsoft.AspNetCore.Mvc;
using System;
using thegame.Models;
using thegame.Services;
using static thegame.Services.FieldGenerator;

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
    public IActionResult Index([FromBody] DifficultDto difficult)
    {
        var gameDto = _fieldGenerator.GenerateNewField(Enum.Parse<Difficult>(difficult.Difficult));

        _repo.AddNewGame(gameDto);
        return Ok(gameDto);
        //var field = _fieldGenerator.sta
        //return Ok(new FieldGenerator().StartNewGame(FieldGenerator.Difficult.Medium));
    }
}