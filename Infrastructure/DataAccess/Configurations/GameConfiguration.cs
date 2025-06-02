using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Games>
    {
        public void Configure(EntityTypeBuilder<Games> builder)
        {
            builder.ToTable("TB_Games");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseMySqlIdentityColumn();
            builder.Property(p => p.DataCriacao).HasColumnType("DATETIME");
            builder.Property(p => p.Nome).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(p => p.Genero).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(p => p.Descricao).HasColumnType("VARCHAR(1000)").IsRequired();
            builder.Property(p => p.Preco).HasColumnType("DECIMAL").IsRequired();
            builder.Property(p => p.Desenvolvedora).HasColumnType("VARCHAR(50)").IsRequired();
            builder.Property(p => p.DataLancamento).HasColumnType("DATETIME");
        }
    }
}
