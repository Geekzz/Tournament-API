using Microsoft.AspNetCore.JsonPatch;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Service.Contracts
{
    public interface IGameService
    {
        public Task<IEnumerable<GameDto>> GetGamesAsync();
        public Task<IEnumerable<GameDto>> GetGameByTitleAsync(string title);
        public Task<GameDto> GetGameByIdAsync(int id);
        public Task<GameDto> CreateGameAsync(GameDto game);
        public Task<GameDto> UpdateGameAsync(int id, GameDto gameDto);
        public Task<GameDto> DeleteGameAsync(int id);
        public Task<GameDto> PatchGameAsync(int id, JsonPatchDocument<GameDto> patchDoc);
        public Task<bool> AnyAsync(int id);
    }
}
