using Blog.Business.Models;
using Blog.Data.Context;
using Blog.Web.Data;
using Microsoft.EntityFrameworkCore;



namespace Blog.Web.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<MeuDbContext>();
            var contextId = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (env.IsDevelopment())
            {
                await context.Database.MigrateAsync();
                await contextId.Database.MigrateAsync();

                await EnsureSeedProducts(context, contextId);
            }
        }

        private static async Task EnsureSeedProducts(MeuDbContext context, ApplicationDbContext contextId)
        {
            if (context.Autores.Any())
                return;

            //Autor

            var idAutor = Guid.NewGuid();

            await contextId.Users.AddAsync(new Microsoft.AspNetCore.Identity.IdentityUser
            {
                Id =  idAutor.ToString(),
                UserName = "marco@imperiumsolucoes.com.br",
                NormalizedUserName = "MARCO@IMPERIUMSOLUCOES.COM.BR",
                Email = "marco@imperiumsolucoes.com.br",
                NormalizedEmail = "MARCO@IMPERIUMSOLUCOES.COM.BR",
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEK2wx+k3kW2104aBWullMN7JJ6VTreIIcBpiyzNVRhRONj2J5GX9ig8EIA9TQcqn9w==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString()
            });

            await contextId.SaveChangesAsync();



            await context.Autores.AddAsync(new Blog.Business.Models.Autor()
            {
                Id = idAutor,
                Nome = "Marco Aurelio Roque",
                Email = "marco@marco.com.br",
                Biografia = "Marco Aurelio Roque é um entusiasta da tecnologia e apaixonado por desenvolvimento de software. Com mais de 15 anos de experiência na área."
            });

            await context.SaveChangesAsync();


            var postagem = new Postagem
            {
                Id = Guid.NewGuid(),
                Titulo = "TESTE 1",
                Conteudo = "é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde o século XVI, quando um impressor desconhecido pegou uma bandeja de tipos e os embaralhou para fazer um livro de modelos de tipos. Lorem Ipsum sobreviveu não só a cinco séculos, como também ao salto para a editoração eletrônica, permanecendo essencialmente inalterado. Se popularizou na década de 60, quando a Letraset lançou decalques contendo passagens de Lorem Ipsum, e mais recentemente quando passou a ser integrado a softwares de editoração eletrônica como Aldus PageMaker",
                DataCriacao = DateTime.Now,
                IdAutor = idAutor
            };

            await context.Postagens.AddAsync(postagem);


            postagem = new Postagem
            {
                Id = Guid.NewGuid(),
                Titulo = "TESTE 2",
                Conteudo = "é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde o século XVI, quando um impressor desconhecido pegou uma bandeja de tipos e os embaralhou para fazer um livro de modelos de tipos. Lorem Ipsum sobreviveu não só a cinco séculos, como também ao salto para a editoração eletrônica, permanecendo essencialmente inalterado. Se popularizou na década de 60, quando a Letraset lançou decalques contendo passagens de Lorem Ipsum, e mais recentemente quando passou a ser integrado a softwares de editoração eletrônica como Aldus PageMaker",
                DataCriacao = DateTime.Now,
                IdAutor = idAutor
            };

            await context.Postagens.AddAsync(postagem);


            postagem = new Postagem
            {
                Id = Guid.NewGuid(),
                Titulo = "TESTE 3",
                Conteudo = "é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde o século XVI, quando um impressor desconhecido pegou uma bandeja de tipos e os embaralhou para fazer um livro de modelos de tipos. Lorem Ipsum sobreviveu não só a cinco séculos, como também ao salto para a editoração eletrônica, permanecendo essencialmente inalterado. Se popularizou na década de 60, quando a Letraset lançou decalques contendo passagens de Lorem Ipsum, e mais recentemente quando passou a ser integrado a softwares de editoração eletrônica como Aldus PageMaker",
                DataCriacao = DateTime.Now,
                IdAutor = idAutor
            };

            await context.Postagens.AddAsync(postagem);


            await context.SaveChangesAsync();


            if (contextId.Users.Any())
                return;

            //






            //await contextId.Users.AddAsync(new IdentityUser
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    UserName = "teste@teste.com",
            //    NormalizedUserName = "TESTE@TESTE.COM",
            //    Email = "teste@teste.com",
            //    NormalizedEmail = "TESTE@TESTE.COM",
            //    AccessFailedCount = 0,
            //    LockoutEnabled = false,
            //    PasswordHash = "AQAAAAIAAYagAAAAEEdWhqiCwW/jZz0hEM7aNjok7IxniahnxKxxO5zsx2TvWs4ht1FUDnYofR8JKsA5UA==",
            //    TwoFactorEnabled = false,
            //    ConcurrencyStamp = Guid.NewGuid().ToString(),
            //    EmailConfirmed = true,
            //    SecurityStamp = Guid.NewGuid().ToString()
            //});

            //await contextId.SaveChangesAsync();
        }
    }
}
