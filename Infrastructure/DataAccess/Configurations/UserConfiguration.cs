using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("TB_User");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnType("INT").UseMySqlIdentityColumn();
            builder.Property(p => p.Email).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(p => p.DataCriacao).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.Password).HasColumnType("VARCHAR(250)").IsRequired();
            builder.Property(p => p.CPF).HasColumnType("VARCHAR(11)").IsRequired();
            builder.Property(p => p.DataNascimento).HasColumnType("DATETIME").IsRequired();
            builder.Property(p => p.Token).HasColumnType("VARCHAR(4000)").IsRequired();
            builder.Property(p => p.Role).HasColumnType("VARCHAR(20)").IsRequired();

        }
    }
}
