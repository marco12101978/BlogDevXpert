using Blog.Data.Context;
using Blog.Data.Models;
using Blog.Web.Data;
using Microsoft.AspNetCore.Identity;
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
            if (context.Authors.Any())
                return;

            //Autor

            var idAutor = Guid.NewGuid();

            await context.Authors.AddAsync(new Blog.Data.Models.Autor()
            {
                IdAutor = idAutor,
                Nome = "Marco Aurelio Roque",
                Email = "marco@marco.com.br",
                Biografia = "Marco Aurelio Roque é um entusiasta da tecnologia e apaixonado por desenvolvimento de software. Com mais de 15 anos de experiência na área."
            });

            await context.SaveChangesAsync();

            //await context.Fornecedores.AddAsync(new Fornecedor()
            //{
            //    Id = idFornecedor,
            //    Nome = "Fornecedor Teste",
            //    Documento = "49445522389",
            //    TipoFornecedor = TipoFornecedor.PessoaFisica,
            //    Ativo = true,
            //    Endereco = new Endereco()
            //    {
            //        Logradouro = "Rua Teste",
            //        Numero = "123",
            //        Complemento = "Complemento",
            //        Bairro = "Teste",
            //        Cep = "03180000",
            //        Cidade = "São Paulo",
            //        Estado = "SP"
            //    }
            //});

            //await context.SaveChangesAsync();

            //if (context.Produtos.Any())
            //    return;

            //await context.Produtos.AddAsync(new Produto()
            //{
            //    Nome = "Livro CSS",
            //    Valor = 50,
            //    Descricao = "Teste",
            //    Ativo = true,
            //    DataCadastro = DateTime.Now,
            //    FornecedorId = idFornecedor
            //});

            //await context.Produtos.AddAsync(new Produto()
            //{
            //    Nome = "Livro jQuery",
            //    Valor = 150,
            //    Descricao = "Teste",
            //    Ativo = true,
            //    DataCadastro = DateTime.Now,
            //    FornecedorId = idFornecedor
            //});

            //await context.Produtos.AddAsync(new Produto()
            //{
            //    Nome = "Livro HTML",
            //    Valor = 90,
            //    Descricao = "Teste",
            //    Ativo = true,
            //    DataCadastro = DateTime.Now,
            //    FornecedorId = idFornecedor
            //});

            //await context.Produtos.AddAsync(new Produto()
            //{
            //    Nome = "Livro Razor",
            //    Valor = 50,
            //    Descricao = "Teste",
            //    Ativo = true,
            //    DataCadastro = DateTime.Now,
            //    FornecedorId = idFornecedor
            //});

            //await context.SaveChangesAsync();

            //if (contextId.Users.Any())
            //    return;

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
