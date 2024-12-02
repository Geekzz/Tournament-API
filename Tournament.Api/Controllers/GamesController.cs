using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public GamesController(IUoW uoW)
        {
            //_context = context;
            _uoW = uoW;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _uoW.GameRepository.GetAllAsync();
            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            //var game = await _context.Games.FindAsync(id);

            //if (game == null)
            //{
            //    return NotFound();
            //}

            //return game;

            var game = await _uoW.GameRepository.GetAsync(id);
            if (game == null) { return NotFound(); }

            return Ok(game);
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
            return Ok(existingGame);

            //_context.Entry(game).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!GameExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            //_context.Games.Add(game);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetGame", new { id = game.Id }, game);

            _uoW.GameRepository.Add(game);
            await _uoW.CompleteAsync();
            //return Ok(game);
            return CreatedAtAction(nameof(PostGame), new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            //var game = await _context.Games.FindAsync(id);
            //if (game == null)
            //{
            //    return NotFound();
            //}

            //_context.Games.Remove(game);
            //await _context.SaveChangesAsync();

            //return NoContent();

            var game = await _uoW.GameRepository.GetAsync(id);
            if (game == null) { return NotFound() ; }

            _uoW.GameRepository.Delete(game);
            await _uoW.CompleteAsync();
            return Ok(game);
        }

        private async Task<bool> GameExists(int id)
        {
            //return _context.Games.Any(e => e.Id == id);
            return await _uoW.GameRepository.AnyAsync(id);
        }
    }
}
