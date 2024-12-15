using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public GamesController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<IActionResult> GetGames()
        {
            var games = await _serviceManager.GameService.GetGamesAsync();
            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetGame(int id)
        {
            var game = await _serviceManager.GameService.GetGameByIdAsync(id);
            if (game == null)
                return NotFound($"Game with ID {id} not found.");

            return Ok(game);
        }

        // GET: api/Games/title
        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetGamesByTitle(string title)
        {
            var games = await _serviceManager.GameService.GetGameByTitleAsync(title);
            if (!games.Any())
                return NotFound($"No games found with the title '{title}'.");

            return Ok(games);
        }

        // POST: api/Games
        [HttpPost]
        public async Task<IActionResult> CreateGame([FromBody] GameDto gameDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdGame = await _serviceManager.GameService.CreateGameAsync(gameDto);
            return CreatedAtAction(nameof(GetGame), new { id = createdGame.Id }, createdGame);
        }

        // PUT: api/Games/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] GameDto gameDto)
        {
            if (id != gameDto.Id)
                return BadRequest("Game ID mismatch.");

            var updatedGame = await _serviceManager.GameService.UpdateGameAsync(id, gameDto);
            if (updatedGame == null)
                return NotFound($"Game with ID {id} not found.");

            return Ok(updatedGame);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var result = await _serviceManager.GameService.DeleteGameAsync(id);
            if (result == null)
                return NotFound($"Game with ID {id} not found.");

            return NoContent(); // 204 No Content on successful delete
        }

        // PATCH: api/Games/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchGame(int id, [FromBody] JsonPatchDocument<GameDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document is null.");

            var patchedGame = await _serviceManager.GameService.PatchGameAsync(id, patchDoc);
            if (patchedGame == null)
                return NotFound($"Game with ID {id} not found.");

            return Ok(patchedGame);
        }
    }
}
