using Core.Entities;
using Core.Entities.GetEntities;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repository
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool Logar(string email, string password, string nome)
        {
            var logar = _dbSet.FirstOrDefault(l => l.Email == email && l.Password == password && l.Nome == nome);

            if(logar != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
