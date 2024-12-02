using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Repositories;
using Bogus.DataSets;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUoW _uoW;

        public TournamentsController(IUoW uoW)
        {
            //_context = context;
            _uoW = uoW;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDetails>>> GetTournament(bool includeGames)
        {
            //var tournaments = await _context.Tournament
            //    .Include(t => t.Games)
            //    .Select(t => new
            //    {
            //        t.Id,
            //        t.Title,
            //        t.StartDate,
            //        Games = t.Games.Select(g => new { g.Id, g.Title })
            //    }).ToListAsync();
            //return Ok(tournaments);
            //return await _uoW.Tournament.ToListAsync();

            var tournaments = includeGames ? await _uoW.TournamentRepository.GetAllAsync() :
                await _uoW.TournamentRepository.GetAllAsync(true);
            return Ok(tournaments);
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDetails>> GetTournament(int id)
        {
            //var tournament = await _context.Tournament.FindAsync(id);

            //if (tournament == null)
            //{
            //    return NotFound();
            //}

            //return tournament;
            var tournament = await _uoW.TournamentRepository.GetAsync(id);
            return Ok(tournament);
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentDetails tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest();
            }

            var existingTournament = await _uoW.TournamentRepository.GetAsync(id);
            if (existingTournament == null) { return NotFound(); }
            existingTournament.Title = tournament.Title;
            existingTournament.StartDate = tournament.StartDate;
            existingTournament.Games = tournament.Games;

            await _uoW.CompleteAsync();
            return Ok(existingTournament);

            //_context.Entry(tournament).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!TournamentExists(id))
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

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournament(TournamentDetails tournament)
        {
            //_context.Tournament.Add(tournament);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTournament", new { id = tournament.Id }, tournament);
            _uoW.TournamentRepository.Add(tournament);
            await _uoW.CompleteAsync();
            //return Ok(tournament);
            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournament);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            //var tournament = await _context.Tournament.FindAsync(id);
            //if (tournament == null)
            //{
            //    return NotFound();
            //}

            //_context.Tournament.Remove(tournament);
            //await _context.SaveChangesAsync();

            //return NoContent();
            var tournament = await _uoW.TournamentRepository.GetAsync(id);
            if(tournament == null) { return NotFound(); };

            _uoW.TournamentRepository.Delete(tournament);
            await _uoW.CompleteAsync();
            return Ok(tournament);
        }

        private async Task<bool> TournamentExists(int id)
        {
            //return _context.Tournament.Any(e => e.Id == id);
            return await _uoW.TournamentRepository.AnyAsync(id);
        }
    }
}
