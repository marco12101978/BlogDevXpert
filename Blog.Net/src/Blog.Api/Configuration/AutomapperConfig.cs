using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Business.Models;

namespace Blog.Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Autor, AutorViewModel>().ReverseMap();
            CreateMap<Postagem, PostagemViewModel>().ReverseMap();
            CreateMap<Comentario, ComentarioViewModel>().ReverseMap();
        }
    }
}
