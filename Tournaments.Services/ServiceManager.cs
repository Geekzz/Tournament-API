using AutoMapper;
using Service.Contracts;
using Tournament.Core.Repositories;
using Tournament.Services;

namespace Tournament.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IGameService> gameService;
        private readonly Lazy<ITournamentService> tournamentService;

        public IGameService GameService => gameService.Value;
        public ITournamentService TournamentService => tournamentService.Value;

        public ServiceManager(IUoW uow, IMapper mapper)
        {
            ArgumentNullException.ThrowIfNull(nameof(uow));

            gameService = new Lazy<IGameService>(() => new GameService(uow, mapper));
            tournamentService = new Lazy<ITournamentService>(() => new TournamentService(uow, mapper));
        }
    }
}
