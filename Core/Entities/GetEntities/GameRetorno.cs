namespace Core.Entities.GetEntities
{
    public class GameRetorno
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string? Nome { get; set; }
        public string? Genero { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string? Desenvolvedora { get; set; }
        public DateTime DataLancamento { get; set; }
        public ICollection<UserGameRetorno>? UserGames { get; set; }

    }
}
