using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using thegame.Models;
using thegame.Services;
using static thegame.Services.FieldGenerator;

namespace thegame.Controllers;

[Route("api/games")]
public class GamesController : Controller
{
    private readonly IGamesRepository _repo;
    private readonly IFieldGenerator _fieldGenerator;
    private readonly IMapper _mapper;

    public GamesController(IGamesRepository repo, IFieldGenerator fieldGenerator, IMapper mapper)
    {
        _repo = repo;
        _fieldGenerator = fieldGenerator;
        _mapper = mapper;
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
    
    [HttpGet("{gameId}")]
    [HttpHead("{gameId}")]
    public IActionResult GetGame(Guid gameId)
    {
        var game = _repo.GetGame(gameId);
        if (game is null)
            return NotFound();
        var mapped = _mapper.Map<GameDto>(game);
        return Ok(mapped);
    }
    
    [HttpDelete("{gameId}")]
    public IActionResult DelGame(Guid gameId)
    {
        _repo.Delete(gameId);
        return NoContent();
    }
}