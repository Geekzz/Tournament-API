using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories
{
    public interface ITournamentRepository
    {
        public Task<IEnumerable<TournamentDetails>> GetAllAsync(bool includeGames = false);
        public Task<TournamentDetails?> GetAsync(int id);
        public Task<bool> AnyAsync(int id);
        public void Add(TournamentDetails tour);
        public void Update(TournamentDetails tour);
        public void Delete(TournamentDetails tour);
        Task<double> CountAsync();
        Task<List<TournamentDetails>> GetPagedAsync(int page, int pageSize, bool includeGames);
    }
}
