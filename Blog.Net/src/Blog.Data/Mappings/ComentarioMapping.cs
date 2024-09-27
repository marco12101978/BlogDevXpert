using Blog.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Mappings
{
    internal class ComentarioMapping : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.Property(c => c.Conteudo)
                   .IsRequired()
                   .HasColumnType("varchar(1000)");

            builder.Property(c => c.DataPostagem)
                   .IsRequired()
                   .HasColumnType("datetime");

            builder.Property(c => c.NomeAutor)
                   .IsRequired()
                   .HasColumnType("varchar(100)");

            builder.Property(c => c.IdPostagem)
                   .IsRequired();

            builder.HasOne(c => c.Postagem)
                   .WithMany(p => p.Comentarios)
                   .HasForeignKey(c => c.IdPostagem)
                   .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}
