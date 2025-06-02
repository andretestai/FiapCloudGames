using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations
{
    public class UserGamesConfiguration : IEntityTypeConfiguration<UserGames>
    {
        public void Configure(EntityTypeBuilder<UserGames> builder)
        {
            builder.ToTable("TB_UserGames");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseMySqlIdentityColumn();
            builder.Property(p => p.DataCriacao).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.UserId).HasColumnType("INT");
            builder.Property(p => p.GameId).HasColumnType("INT");

            builder.HasOne(p => p.User)
                .WithMany(p => p.UserGames)
                .HasPrincipalKey(u => u.Id);

            builder.HasOne(p => p.Game)
                .WithMany(g => g.UserGames)
                .HasPrincipalKey(g => g.Id);
                
        }
    }
}
