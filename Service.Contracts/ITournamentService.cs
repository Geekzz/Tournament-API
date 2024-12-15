using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Request;

namespace Service.Contracts
{
    public interface ITournamentService
    {
        public Task<PagedResult<TournamentDto>> GetAllTournamentsAsync(int page, int pageSize, bool includeGames = false);
        public Task<TournamentDto> GetTournamentByIdAsync(int id);
        public Task<TournamentDto> CreateTournamentAsync(TournamentDto tournament);
        public Task<TournamentDto> UpdateTournamentAsync(int id, TournamentDto tournamentDto);
        public Task<TournamentDto> DeleteTournamentAsync(int id);
        public Task<TournamentDto> PatchTournamentAsync(int id, JsonPatchDocument<TournamentDto> patchDoc);
        public Task<bool> AnyAsync(int id);
    }
}
