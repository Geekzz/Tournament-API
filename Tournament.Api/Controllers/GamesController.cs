using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
            var games = await _uoW.GameRepository.GetAllAsync();
            var gamesDtos = _mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gamesDtos);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _uoW.GameRepository.GetAsync(id);
            if (game == null) { return NotFound(); }
            var gamesDto = _mapper.Map<GameDto>(game);

            return Ok(gamesDto);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            var existingGame = await _uoW.GameRepository.GetAsync(id);
            if (existingGame == null) { return NotFound(); }
            existingGame.Title = game.Title;
            existingGame.Time = game.Time;
            existingGame.TournamentDetailsId = game.TournamentDetailsId;

            await _uoW.CompleteAsync();
            var gameDto = _mapper.Map<GameDto>(existingGame);
            return Ok(gameDto);
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            _uoW.GameRepository.Add(game);
            await _uoW.CompleteAsync();
            var gameDto = _mapper.Map<GameDto>(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameDto);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _uoW.GameRepository.GetAsync(id);
            if (game == null) { return NotFound() ; }

            _uoW.GameRepository.Delete(game);
            await _uoW.CompleteAsync();
            var gameDto = _mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        private async Task<bool> GameExists(int id)
        {
            return await _uoW.GameRepository.AnyAsync(id);
        }
    }
}
