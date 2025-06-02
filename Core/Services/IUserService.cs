using Core.Entities.GetEntities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;

namespace Core.Services
{
    public interface IUserService
    {
        void Cadastrar(UserInsert user);
        void Alterar(UserUpdate user);
        void Deletar(int id);
        IList<UserRetorno> ObterTodos();
        UserRetorno ObterId(int id);
        UserRetorno BuscarJogosUser(int id);
        bool Logar(string email, string password, string nome);

    }
}
