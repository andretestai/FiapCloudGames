using Core.Entities;
using Core.Entities.GetEntities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;

namespace Core.Services
{
    public interface IUserGameService
    {
        void Cadastrar(UserGameInsert userGame);
        void Alterar(UserGameUpdate userGame);
        void Deletar(int id);
        IList<UserGameRetorno> ObterTodos();
        UserGameRetorno ObterId(int id);
    }
}
