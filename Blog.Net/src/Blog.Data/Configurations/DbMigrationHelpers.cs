using Blog.Business.Models;
using Blog.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blog.Data.Configurations
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

                await EnsureSeedProducts(serviceProvider, context, contextId);
            }
        }

        private static async Task EnsureSeedProducts(IServiceProvider serviceProvider, MeuDbContext context, ApplicationDbContext contextId)
        {
            if (contextId.Users.Any())
                return;


            #region Usuario Identity

            var idAutor = Guid.NewGuid();

            var user = new Microsoft.AspNetCore.Identity.IdentityUser
            {
                Id = idAutor.ToString(),
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
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            await contextId.Users.AddAsync(user);

            await contextId.SaveChangesAsync();

            #endregion

            #region Roles

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


            string[] roleNames = { "Admin", "User", "Manager" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var _userManager = serviceProvider.GetService<UserManager<Microsoft.AspNetCore.Identity.IdentityUser>>();

            user = await _userManager.FindByIdAsync(idAutor.ToString());

            await _userManager.AddToRoleAsync(user, "Admin");


            #endregion

            #region Autor

            await context.Autores.AddAsync(new Blog.Business.Models.Autor()
            {
                Id = idAutor,
                Nome = "marco@imperiumsolucoes.com.br",
                Email = "marco@imperiumsolucoes.com.br",
                Biografia = ""
            });

            await context.SaveChangesAsync();

            #endregion

            #region Postagens
            Postagem postagem;

            #region Postagem 1
            var idPostagem1 = Guid.NewGuid();
            postagem = new Postagem
            {
                Id = idPostagem1,
                Titulo = "5 Dicas para Aumentar sua Produtividade no Trabalho",
                Conteudo = "Introdução\r\nNo mundo acelerado de hoje, ser produtivo é essencial para alcançar suas metas profissionais. Aqui estão cinco dicas práticas para ajudar você a maximizar sua eficiência no trabalho.\r\n\r\n1. Estabeleça Metas Claras\r\nDefina objetivos diários, semanais e mensais. Quando você tem um caminho claro a seguir, fica mais fácil manter o foco.\r\n\r\n2. Use a Técnica Pomodoro\r\nEssa técnica consiste em trabalhar por 25 minutos seguidos e, em seguida, fazer uma pausa de 5 minutos. Isso ajuda a manter sua mente fresca e produtiva.\r\n\r\n3. Elimine Distrações\r\nIdentifique e minimize as distrações ao seu redor. Isso pode incluir desativar notificações de celular ou criar um espaço de trabalho mais organizado.\r\n\r\n4. Priorize Tarefas\r\nUtilize a matriz de Eisenhower para classificar suas tarefas em urgentes e importantes. Isso ajudará você a focar no que realmente importa.\r\n\r\n5. Revise e Ajuste\r\nAo final de cada semana, faça uma revisão das suas conquistas e ajuste suas metas para a próxima. Aprender com a experiência é fundamental para melhorar sua produtividade.\r\n\r\nConclusão\r\nImplementando essas dicas no seu dia a dia, você pode notar uma grande diferença na sua produtividade. Comece hoje mesmo!",
                DataCriacao = DateTime.Now.AddDays(-1),
                IdAutor = idAutor
            };

            var cont = postagem.Conteudo.Length;

            await context.Postagens.AddAsync(postagem);

            await context.SaveChangesAsync();

            #endregion

            #region Postagem 2
            var idPostagem2 = Guid.NewGuid();
            postagem = new Postagem
            {
                Id = idPostagem2,
                Titulo = "Os Benefícios da Meditação para a Saúde Mental",
                Conteudo = "Introdução\r\nNos últimos anos, a meditação ganhou destaque como uma prática eficaz para melhorar a saúde mental. Neste post, discutiremos os principais benefícios dessa técnica.\r\n\r\n1. Redução do Estresse\r\nA meditação ajuda a reduzir os níveis de cortisol, o hormônio do estresse, proporcionando uma sensação de calma e relaxamento.\r\n\r\n2. Aumento da Concentração\r\nPraticar meditação regularmente pode melhorar sua capacidade de concentração, ajudando você a se manter focado em tarefas importantes.\r\n\r\n3. Melhora no Sono\r\nA meditação pode ajudar a acalmar a mente antes de dormir, resultando em um sono mais profundo e reparador.\r\n\r\n4. Regulação das Emoções\r\nA prática regular de meditação pode aumentar a autoconsciência e melhorar a regulação emocional, tornando mais fácil lidar com situações difíceis.\r\n\r\n5. Promoção do Bem-Estar Geral\r\nMeditar pode levar a um aumento geral do bem-estar e da satisfação com a vida, promovendo uma perspectiva mais positiva.\r\n\r\nConclusão\r\nIncorporar a meditação na sua rotina diária pode trazer muitos benefícios para a sua saúde mental. Experimente começar com apenas alguns minutos por dia!",
                DataCriacao = DateTime.Now.AddDays(-2),
                IdAutor = idAutor
            };
            await context.Postagens.AddAsync(postagem);

            await context.SaveChangesAsync();
            #endregion

            #region Postagem 3
            var idPostagem3 = Guid.NewGuid();
            postagem = new Postagem
            {
                Id = idPostagem3,
                Titulo = "10 Receitas Rápidas e Fáceis para o Almoço",
                Conteudo = "Introdução\r\nSe você está sempre correndo e precisa de ideias para almoços rápidos, este post é para você! Aqui estão 10 receitas que são simples e deliciosas.\r\n\r\n1. Salada de Quinoa e Legumes\r\nMisture quinoa cozida, tomate, pepino e abacate. Tempere com azeite e limão.\r\n\r\n2. Wrap de Frango Grelhado\r\nRecheie uma tortilla com frango grelhado, alface, tomate e um pouco de molho.\r\n\r\n3. Omelete de Espinafre\r\nBata ovos com espinafre e queijo feta. Cozinhe em uma frigideira até ficar firme.\r\n\r\n4. Arroz de Couve-Flor\r\nProcesse couve-flor até ficar com textura de arroz. Refogue com alho e cebola.\r\n\r\n5. Tacos de Peixe\r\nUse filés de peixe grelhados, coloque em tortilhas e adicione repolho e molho de iogurte.\r\n\r\n6. Sopa de Lentilha\r\nCozinhe lentilhas com cenoura, cebola e temperos. Uma opção nutritiva e reconfortante!\r\n\r\n7. Salada de Grão-de-Bico\r\nMisture grão-de-bico cozido com cebola roxa, salsa e molho de azeite.\r\n\r\n8. Macarrão ao Pesto\r\nCozinhe macarrão e misture com pesto e vegetais grelhados.\r\n\r\n9. Frango à Parmegiana Rápido\r\nCubra peitos de frango com molho de tomate e queijo, e leve ao forno.\r\n\r\n10. Smoothie Verde\r\nBata espinafre, banana, maçã e água de coco para uma opção refrescante.\r\n\r\nConclusão\r\nEssas receitas são rápidas, práticas e perfeitas para o almoço. Experimente e descubra novas combinações que você vai adorar!",
                DataCriacao = DateTime.Now.AddDays(-5),
                IdAutor = idAutor
            };

            await context.Postagens.AddAsync(postagem);
            await context.SaveChangesAsync();

            #endregion

            #endregion

            #region Comentarios
            Comentario comentario;

            #region Comentarios Postagem 1

            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Jose da Silva",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now.AddDays(-1),
                IdPostagem = idPostagem1,
                Conteudo = "Excelente artigo! Estava procurando exatamente por isso. As dicas que você deu serão muito úteis para o meu próximo projeto. Parabéns pelo conteúdo de qualidade!"
            };
            await context.Comentarios.AddAsync(comentario);


            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Carlos Mendes",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now.AddDays(-1),
                IdPostagem = idPostagem1,
                Conteudo = "Gostei muito da forma como você abordou o tema. É sempre bom ver conteúdo técnico bem explicado e de fácil compreensão. Continue com o ótimo trabalho!"
            };
            await context.Comentarios.AddAsync(comentario);

            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "João Pereira",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now.AddDays(-1),
                IdPostagem = idPostagem1,
                Conteudo = "Achei a explicação um pouco confusa em alguns pontos, mas com certeza agregou valor ao que eu já sabia. Vou continuar acompanhando seu blog para aprender mais."
            };
            await context.Comentarios.AddAsync(comentario);


            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Ana Costa",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now,
                IdPostagem = idPostagem1,
                Conteudo = "Uau, adorei o post! Consegui entender de maneira clara conceitos que sempre achei complicados. Obrigada por compartilhar esse conhecimento."
            };
            await context.Comentarios.AddAsync(comentario);


            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Lucas Oliveira",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now,
                IdPostagem = idPostagem1,
                Conteudo = "O conteúdo está bem completo, mas acho que seria interessante incluir exemplos práticos para facilitar o entendimento. De qualquer forma, excelente artigo!"
            };
            await context.Comentarios.AddAsync(comentario);

            #endregion


            #region Comentarios Postagem 2

            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Fernanda Rocha",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now.AddDays(-2),
                IdPostagem = idPostagem2,
                Conteudo = "Artigo incrível! As informações são super relevantes e a maneira como você explicou tudo faz com que seja fácil de entender. Estou ansiosa para aplicar essas dicas!"
            };
            await context.Comentarios.AddAsync(comentario);


            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Ricardo Almeida",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now.AddDays(-1),
                IdPostagem = idPostagem2,
                Conteudo = "Gostei bastante do seu ponto de vista. É sempre bom ter diferentes perspectivas sobre o assunto. Vou compartilhar com meus colegas!"
            };
            await context.Comentarios.AddAsync(comentario);


            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Sofia Martins",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now.AddDays(-2),
                IdPostagem = idPostagem2,
                Conteudo = "Ótima leitura! Achei os gráficos e imagens que você usou muito úteis para complementar o texto. Isso realmente ajuda a visualizar as informações."
            };
            await context.Comentarios.AddAsync(comentario);

            #endregion


            #region Comentario Postagem 3

            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Eduardo Lima",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now.AddDays(-4),
                IdPostagem = idPostagem3,
                Conteudo = "Parabéns pelo conteúdo! Você sempre traz temas interessantes e bem pesquisados. Espero que continue postando regularmente!"
            };
            await context.Comentarios.AddAsync(comentario);

            comentario = new Comentario
            {
                Id = Guid.NewGuid(),
                NomeAutor = "Tatiane Sousa",
                IdAutor = idAutor,
                DataPostagem = DateTime.Now.AddDays(-3),
                IdPostagem = idPostagem3,
                Conteudo = "Esse artigo chegou na hora certa! Estava com dificuldades em entender alguns conceitos e você conseguiu esclarecer tudo de forma simples. Muito obrigada!"
            };
            await context.Comentarios.AddAsync(comentario);

            #endregion


            await context.SaveChangesAsync();

            #endregion
        }
    }
}
