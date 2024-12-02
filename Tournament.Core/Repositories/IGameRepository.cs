using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories
{
    public interface IGameRepository
    {
        public Task<IEnumerable<Game>> GetAllAsync();
        public Task<Game?> GetAsync(int id);
        public Task<bool> AnyAsync(int id);
        public void Add(Game tour);
        public void Update(Game tour);
        public void Delete(Game tour);
    }
}
