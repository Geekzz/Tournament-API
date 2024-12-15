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

namespace Tournament.Services
{
    public class GameService : IGameService
    {
        private IUoW _UoW;
        private readonly IMapper _mapper;

        public GameService(IUoW uoW, IMapper mapper)
        {
            _UoW = uoW;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameDto>> GetGamesAsync()
        {
            return _mapper.Map<IEnumerable<GameDto>>(await _UoW.GameRepository.GetAllAsync());
        }

        public async Task<GameDto> GetGameByIdAsync(int id)
        {
            Game? game = await _UoW.GameRepository.GetAsync(id);
            if (game == null)
            {
                throw new KeyNotFoundException($"Game with ID {id} was not found.");
            }

            return _mapper.Map<GameDto>(game);
        }

        public async Task<IEnumerable<GameDto>> GetGameByTitleAsync(string title)
        {
            return _mapper.Map<IEnumerable<GameDto>>(await _UoW.GameRepository.GetByTitleAsync(title));
        }

        public async Task<GameDto> UpdateGameAsync(int id, GameDto gameDto)
        {
            var existingGame = await _UoW.GameRepository.GetAsync(id);
            if (existingGame == null)
            {
                // Handle the case where the game is not found
                return null;
            }
            _mapper.Map(gameDto, existingGame); // Update fields in the existing entity
            _UoW.GameRepository.Update(existingGame);
            await _UoW.CompleteAsync();

            return _mapper.Map<GameDto>(existingGame);
        }

        public async Task<GameDto> DeleteGameAsync(int id)
        {
            var gameEntity = await _UoW.GameRepository.GetAsync(id);
            if (gameEntity == null)
            {
                // Handle the case where the game is not found
                return null;
            }

            _UoW.GameRepository.Delete(gameEntity);
            await _UoW.CompleteAsync();

            return _mapper.Map<GameDto>(gameEntity);
        }

        public async Task<GameDto> CreateGameAsync(GameDto gameDto)
        {
            // Check if the tournament already has 10 games
            var gameCount = await _UoW.GameRepository.CountByTournamentIdAsync(gameDto.Id);
            if (gameCount >= 10)
            {
                throw new InvalidOperationException("A tournament cannot have more than 10 games.");
            }

            var gameEntity = _mapper.Map<Game>(gameDto);

            _UoW.GameRepository.Add(gameEntity);

            await _UoW.CompleteAsync();

            var createdGame = await _UoW.GameRepository.GetAsync(gameEntity.Id);

            return _mapper.Map<GameDto>(createdGame);
        }



        public async Task<GameDto> PatchGameAsync(int id, JsonPatchDocument<GameDto> patchDoc)
        {
            var existingGame = await _UoW.GameRepository.GetAsync(id);
            if (existingGame == null)
            {
                // Handle the case where the game is not found
                return null;
            }

            var gameDto = _mapper.Map<GameDto>(existingGame);  // Map the existing game entity to a DTO

            // Apply the patch document to the DTO
            patchDoc.ApplyTo(gameDto);

            // Map the patched DTO back to the game entity
            _mapper.Map(gameDto, existingGame);

            // Update the entity in the repository
            _UoW.GameRepository.Update(existingGame);
            await _UoW.CompleteAsync();

            return _mapper.Map<GameDto>(existingGame);  // Return the updated DTO
        }

        public async Task<bool> AnyAsync(int id)
        {
            var game = await _UoW.GameRepository.GetAsync(id);
            return game != null;
        }
    }
}
