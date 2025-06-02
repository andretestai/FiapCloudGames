using Core.Entities;
using Core.Repository;

namespace Infrastructure.DataAccess.Repository
{
    public class UserGamesRepository : EFRepository<UserGames>, IUserGameRepository
    {
        public UserGamesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
