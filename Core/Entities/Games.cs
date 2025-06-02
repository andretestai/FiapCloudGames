namespace Core.Entities
{
    public class Games : EntityBase
    {
        public required string Nome { get; set; }
        public required string Genero { get; set; }
        public required string Descricao { get; set; }
        public required decimal Preco { get; set; }
        public required string Desenvolvedora { get; set; }
        public required DateTime DataLancamento { get; set; }

        public virtual ICollection<UserGames>? UserGames { get; set; }
    }
}
