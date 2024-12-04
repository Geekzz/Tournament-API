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
using AutoMapper;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUoW _uoW;
        private readonly IMapper _mapper;

        public TournamentsController(IUoW uoW, IMapper mapper)
        {
            //_context = context;
            _uoW = uoW;
            _mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDetails>>> GetTournament(bool includeGames)
        {
            try
            {
                var tournaments = includeGames ? await _uoW.TournamentRepository.GetAllAsync() :
                    await _uoW.TournamentRepository.GetAllAsync(true);

                var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
                return Ok(tournamentDtos);
            }
            catch
            {
                return StatusCode(500, "Failed to retrieve tournaments from the database.");
            }
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDetails>> GetTournament(int id)
        {
            var tournament = await _uoW.TournamentRepository.GetAsync(id);
            if (tournament == null)
                return NotFound();
            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            return Ok(tournamentDto);
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentDetails tournament)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != tournament.Id)
                return BadRequest();

            var existingTournament = await _uoW.TournamentRepository.GetAsync(id);
            if (existingTournament == null)
                return NotFound();

            existingTournament.Title = tournament.Title;
            existingTournament.StartDate = tournament.StartDate;
            existingTournament.Games = tournament.Games;

            try
            {
                await _uoW.CompleteAsync();
                return Ok(_mapper.Map<TournamentDto>(existingTournament));
            }
            catch
            {
                return StatusCode(500, "Failed to save the tournament to the database.");
            }
        }

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TournamentDetails>> PostTournament(TournamentDetails tournament)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tournamentEntity = _mapper.Map<TournamentDetails>(tournament);

            try
            {
                _uoW.TournamentRepository.Add(tournamentEntity);
                await _uoW.CompleteAsync();
            }
            catch
            {
                return StatusCode(500, "Failed to save the tournament to the database.");
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournamentEntity);
            return CreatedAtAction(nameof(GetTournament), new { id = tournamentEntity.Id }, tournamentDto);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _uoW.TournamentRepository.GetAsync(id);
            if (tournament == null)
                return NotFound();

            try
            {
                _uoW.TournamentRepository.Delete(tournament);
                await _uoW.CompleteAsync();
                return Ok(_mapper.Map<TournamentDto>(tournament));
            }
            catch
            {
                return StatusCode(500, "Failed to delete the tournament from the database.");
            }
        }

        private async Task<bool> TournamentExists(int id)
        {
            //return _context.Tournament.Any(e => e.Id == id);
            return await _uoW.TournamentRepository.AnyAsync(id);
        }

        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId,
            JsonPatchDocument<TournamentDto> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
                return BadRequest("Patch document is null.");

            var tournament = await _uoW.TournamentRepository.GetAsync(tournamentId);
            if (tournament == null)
                return NotFound();

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);

            jsonPatchDocument.ApplyTo(tournamentDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mapper.Map(tournamentDto, tournament);
            await _uoW.CompleteAsync();

            return Ok(tournamentDto);
        }
    }
}
