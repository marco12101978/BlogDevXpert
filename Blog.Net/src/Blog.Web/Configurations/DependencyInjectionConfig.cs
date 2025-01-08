using Blog.Business.Interfaces;
using Blog.Business.Notificacoes;
using Blog.Business.Services;
using Blog.Data.Context;
using Blog.Data.Repository;
using Blog.Web.IdentityUser;

namespace Blog.Web.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAppIdentityUser, AppIdentityUser>();
            services.AddScoped<INotificador, Notificador>();

            services.AddScoped<MeuDbContext>();

            services.AddScoped<IAutorRepository, AutorRepository>();
            services.AddScoped<IAutorService, AutorService>();

            services.AddScoped<IPostagemRepository, PostagemRepository>();
            services.AddScoped<IPostagemService, PostagemService>();


            services.AddScoped<IComentarioRepository, ComentarioRepository>();
            services.AddScoped<IComentarioService, ComentarioService>();

            return services;
        }
    }
}
