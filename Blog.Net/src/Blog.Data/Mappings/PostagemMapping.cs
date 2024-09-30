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
    public class PostagemMapping : IEntityTypeConfiguration<Postagem>
    {
        public void Configure(EntityTypeBuilder<Postagem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Titulo)
                   .IsRequired()
                   .HasColumnType("varchar(200)");

            builder.Property(p => p.Conteudo)
                   .IsRequired()
                   .HasColumnType("varchar(max)");

            builder.Property(p => p.DataCriacao)
                    .IsRequired()
                    .HasColumnType("datetime");

            builder.Property(p => p.DataCriacao)
                    .HasColumnType("datetime");

            builder.Property(p => p.IdAutor)
                    .IsRequired();  

            builder.HasOne(p => p.Autor)
                   .WithMany(a => a.Postagens)  
                   .HasForeignKey(p => p.IdAutor)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Postagem");
        }
    }
}
