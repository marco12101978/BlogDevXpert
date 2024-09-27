using Blog.Business.Intefaces;
using Blog.Business.Notificacoes;
using Blog.Business.Services;
using Blog.Data.Context;
using Blog.Data.Repository;

namespace Blog.Web.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MeuDbContext>();
            services.AddScoped<IAutorRepository, AutorRepository>();
            services.AddScoped<IPostagemRepository, PostagemRepository>();
            services.AddScoped<IComentarioRepository, ComentarioRepository>();

            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<IAutorService, AutorService>();
            services.AddScoped<IPostagemRepository, PostagemRepository>();
            services.AddScoped<IComentarioService, ComentarioService>();

            return services;
        }
    }
}
