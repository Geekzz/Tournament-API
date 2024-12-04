using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TournamentApiContext _context;

        public GameRepository(TournamentApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetByTitleAsync(string title)
        {
            return await _context.Games
                .Where(g => g.Title == title)
                .ToListAsync();
        }

        public async Task<Game?> GetAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }
        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Games.AnyAsync(g => g.Id == id);
        }
        public async void Add(Game tour)
        {
            await _context.Games.AddAsync(tour);
        }
        public void Update(Game tour)
        {
            _context.Games.Update(tour);
        }
        public void Delete(Game tour)
        {
            _context.Games.Remove(tour);
        }
    }
}
