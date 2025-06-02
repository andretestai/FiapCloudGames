namespace Core.Entities
{
    public class User : EntityBase
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string CPF { get; set; }
        public required DateTime DataNascimento { get; set; }
        public required string Token { get; set; }
        public required string Role { get; set; }
        public virtual ICollection<UserGames>? UserGames { get; set; }
    }
}
