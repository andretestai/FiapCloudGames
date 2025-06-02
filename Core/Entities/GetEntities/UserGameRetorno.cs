namespace Core.Entities.GetEntities
{
    public class UserGameRetorno
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public GameRetorno? Game { get; set; }
        public UserRetorno? User { get; set; }
    }
}
