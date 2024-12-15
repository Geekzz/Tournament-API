using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Core.Request;

namespace Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        private IUoW _UoW;
        private readonly IMapper _mapper;

        public TournamentService(IUoW uoW, IMapper mapper)
        {
            _UoW = uoW;
            _mapper = mapper;
        }

        public async Task<PagedResult<TournamentDto>> GetAllTournamentsAsync(int page, int pageSize, bool includeGames = false)
        {
            pageSize = Math.Min(pageSize, 100); // Clamp pageSize to a maximum of 100
            page = Math.Max(page, 1);          // Ensure page is at least 1

            var totalItems = await _UoW.TournamentRepository.CountAsync();
            var tournaments = await _UoW.TournamentRepository.GetPagedAsync(page, pageSize, includeGames);

            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);

            return new PagedResult<TournamentDto>
            {
                Items = tournamentDtos,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                PageSize = pageSize,
                CurrentPage = page,
                TotalItems = (int)totalItems
            };
        }



        public async Task<TournamentDto> GetTournamentByIdAsync(int id)
        {
            var tournament = await _UoW.TournamentRepository.GetAsync(id);
            if (tournament == null) throw new KeyNotFoundException("Tournament not found");

            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<TournamentDto> CreateTournamentAsync(TournamentDto tournament)
        {
            var tour_map = _mapper.Map<TournamentDetails>(tournament);
            _UoW.TournamentRepository.Add(tour_map);
            await _UoW.CompleteAsync();

            return _mapper.Map<TournamentDto>(tour_map);
        }

        public async Task<TournamentDto> UpdateTournamentAsync(int id, TournamentDto tournamentDto)
        {
            var existingTournament = await _UoW.TournamentRepository.GetAsync(id);
            if (existingTournament == null) throw new KeyNotFoundException("Tournament not found");

            _mapper.Map(tournamentDto, existingTournament);
            _UoW.TournamentRepository.Update(existingTournament);
            await _UoW.CompleteAsync();

            return _mapper.Map<TournamentDto>(existingTournament);
        }

        public async Task<TournamentDto> DeleteTournamentAsync(int id)
        {
            var tournament = await _UoW.TournamentRepository.GetAsync(id);
            if (tournament == null) throw new KeyNotFoundException("Tournament not found");

            _UoW.TournamentRepository.Delete(tournament);
            await _UoW.CompleteAsync();

            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<TournamentDto> PatchTournamentAsync(int id, JsonPatchDocument<TournamentDto> patchDoc)
        {
            var tournament = await _UoW.TournamentRepository.GetAsync(id);
            if (tournament == null) throw new KeyNotFoundException("Tournament not found");

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            patchDoc.ApplyTo(tournamentDto);

            _mapper.Map(tournamentDto, tournament);
            _UoW.TournamentRepository.Update(tournament);
            await _UoW.CompleteAsync();

            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<bool> AnyAsync(int id)
        {
            var tour = await _UoW.TournamentRepository.GetAsync(id);
            return tour != null;
        }
    }
}
