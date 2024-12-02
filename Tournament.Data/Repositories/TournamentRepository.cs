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
    public class TournamentRepository : ITournamentRepository
    {
        private readonly TournamentApiContext _context;

        public TournamentRepository(TournamentApiContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TournamentDetails>> GetAllAsync(bool includeGames = false) 
        {
            return includeGames ? await _context.Tournament.ToListAsync() :
                await _context.Tournament.Include(t => t.Games).ToListAsync();
        }

        public async Task<TournamentDetails?> GetAsync(int id)
        {
            return await _context.Tournament.FindAsync(id);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.Tournament.AnyAsync(t => t.Id == id);
        }
        public async void Add(TournamentDetails tour)
        {
            await _context.AddAsync(tour);
        }
        public void Update(TournamentDetails tour)
        {
            _context.Tournament.Update(tour);
        }
        public void Delete(TournamentDetails tour)
        {
            _context.Tournament.Remove(tour);
        }
    }
}
