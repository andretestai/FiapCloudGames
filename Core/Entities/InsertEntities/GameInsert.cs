namespace Core.Entities.InsertEntities
{
    public class GameInsert
    {
        public string? Nome { get; set; }
        public string? Genero { get; set; }
        public string? Descricao { get; set; }
        public decimal? Preco { get; set; }
        public string? Desenvolvedora { get; set; }
        public DateTime? DataLancamento { get; set; }
    }
}
