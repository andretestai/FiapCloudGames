namespace Core.Entities
{
    public class UserGames : EntityBase
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public virtual Games? Game { get; set; }
        public virtual User? User { get; set; }
    }
}
