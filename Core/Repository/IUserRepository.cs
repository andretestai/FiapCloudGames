using Core.Entities;

namespace Core.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        bool Logar(string email, string password, string nome);
    }
}
