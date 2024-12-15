using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using AutoMapper;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;

namespace Tournament.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TournamentsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<IActionResult> GetTournaments(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] bool includeGames = false)
        {
            // Validate pagination parameters
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid Pagination Parameters",
                    Detail = "Both page and pageSize must be positive integers.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            // Call the service layer for paginated results
            var pagedResult = await _serviceManager.TournamentService.GetAllTournamentsAsync(page, pageSize, includeGames);

            // Return the paged result as the response
            return Ok(pagedResult);
        }



        // GET: api/Tournaments/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTournament(int id)
        {
            var tournament = await _serviceManager.TournamentService.GetTournamentByIdAsync(id);
            if (tournament == null)
                return NotFound($"Tournament with ID {id} not found.");

            return Ok(tournament);
        }

        // POST: api/Tournaments
        [HttpPost]
        public async Task<IActionResult> CreateTournament([FromBody] TournamentDto tournamentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTournament = await _serviceManager.TournamentService.CreateTournamentAsync(tournamentDto);
            return CreatedAtAction(nameof(GetTournament), new { id = createdTournament.Id }, createdTournament);
        }

        // PUT: api/Tournaments/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTournament(int id, [FromBody] TournamentDto tournamentDto)
        {
            if (id != tournamentDto.Id)
                return BadRequest("Tournament ID mismatch.");

            var updatedTournament = await _serviceManager.TournamentService.UpdateTournamentAsync(id, tournamentDto);
            if (updatedTournament == null)
                return NotFound($"Tournament with ID {id} not found.");

            return Ok(updatedTournament);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var success = await _serviceManager.TournamentService.DeleteTournamentAsync(id);
            if (success == null)
                return NotFound($"Tournament with ID {id} not found.");

            return NoContent(); // 204 No Content
        }

        // PATCH: api/Tournaments/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchTournament(int id, [FromBody] JsonPatchDocument<TournamentDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document is null.");

            var patchedTournament = await _serviceManager.TournamentService.PatchTournamentAsync(id, patchDoc);
            if (patchedTournament == null)
                return NotFound($"Tournament with ID {id} not found.");

            return Ok(patchedTournament);
        }
    }
}
