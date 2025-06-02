namespace Core.Entities.GetEntities
{
    public class UserRetorno
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public ICollection<UserGameRetorno>? UserGames { get; set; }

    }
}
