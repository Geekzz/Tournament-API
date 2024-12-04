using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;
using Tournament.Data.Repositories;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUoW _uoW;
        private readonly IMapper _mapper;

        public GamesController(IUoW uoW, IMapper mapper)
        {
            //_context = context;
            _uoW = uoW;
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            try
            {
                var games = await _uoW.GameRepository.GetAllAsync();
                if (games == null || !games.Any())
                    return NotFound();

                var gamesDtos = _mapper.Map<IEnumerable<GameDto>>(games);
                return Ok(gamesDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the games.");
            }
        }

        // GET: api/Games/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            try
            {
                var game = await _uoW.GameRepository.GetAsync(id);
                if (game == null)
                    return NotFound();

                var gamesDto = _mapper.Map<GameDto>(game);
                return Ok(gamesDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the game.");
            }
        }

        [HttpGet("{title}")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesByTitle(string title)
        {
            try
            {
                var games = await _uoW.GameRepository.GetByTitleAsync(title);

                if (!games.Any())
                    return NotFound($"No games found with the title '{title}'.");

                var gamesDtos = _mapper.Map<IEnumerable<GameDto>>(games);
                return Ok(gamesDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching games.");
            }
        }



        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != game.Id)
                return BadRequest("Game ID mismatch.");

            var existingGame = await _uoW.GameRepository.GetAsync(id);
            if (existingGame == null)
                return NotFound();

            try
            {
                existingGame.Title = game.Title;
                existingGame.Time = game.Time;
                existingGame.TournamentDetailsId = game.TournamentDetailsId;

                await _uoW.CompleteAsync();
                var gameDto = _mapper.Map<GameDto>(existingGame);
                return Ok(gameDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while updating the game.");
            }
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                _uoW.GameRepository.Add(game);
                await _uoW.CompleteAsync();

                var gameDto = _mapper.Map<GameDto>(game);
                return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while saving the game.");
            }
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _uoW.GameRepository.GetAsync(id);
            if (game == null)
                return NotFound();

            try
            {
                _uoW.GameRepository.Delete(game);
                await _uoW.CompleteAsync();

                var gameDto = _mapper.Map<GameDto>(game);
                return Ok(gameDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while deleting the game.");
            }
        }

        private async Task<bool> GameExists(int id)
        {
            return await _uoW.GameRepository.AnyAsync(id);
        }


        [HttpPatch("{gameId}")]
        public async Task<ActionResult<GameDto>> PatchGame(int gameId,
            JsonPatchDocument<GameDto> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
                return BadRequest("Patch document is null.");

            var game = await _uoW.GameRepository.GetAsync(gameId);
            if (game == null)
                return NotFound();

            var gameDto = _mapper.Map<GameDto>(game);

            jsonPatchDocument.ApplyTo(gameDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mapper.Map(gameDto, game);
            await _uoW.CompleteAsync();

            return Ok(gameDto);
        }

    }
}
