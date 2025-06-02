using Core.Entities;
using Core.Repository;

namespace Infrastructure.DataAccess.Repository
{
    public class GamesRepository : EFRepository<Games>, IGameRepository
    {
        public GamesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
