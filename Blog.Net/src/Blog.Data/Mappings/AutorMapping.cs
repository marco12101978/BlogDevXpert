using Blog.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    internal class AutorMapping : IEntityTypeConfiguration<Autor>
    {
        public void Configure(EntityTypeBuilder<Autor> builder)
        {
            builder.Property(a => a.Nome)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Biografia)
                   .HasMaxLength(1000);

            builder.HasMany(a => a.Postagens)
                   .WithOne(p => p.Autor)
                   .HasForeignKey(p => p.IdAutor)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Autor");
        }
    }
}
