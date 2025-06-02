using Core.Entities.GetEntities;
using Core.Entities.InsertEntities;
using Core.Entities.UpdateEntity;

namespace Core.Services
{
    public interface IGameService
    {
        void Cadastrar(GameInsert game);
        void Alterar(GameUpdate game);
        void Apagar(int id);
        IList<GameRetorno> ObterTodos();
        GameRetorno ObterId(int id);
    }
}
