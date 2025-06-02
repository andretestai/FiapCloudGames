using Core.Entities;

namespace Core.Repository
{
    public interface IRepository<T> where T : EntityBase
    {
        IList<T> ObterTodos();
        T ObterId(int id);
        void Cadastrar(T entidade);
        void Alterar(T entidade);
        void Deletar(int id);
    }
}
