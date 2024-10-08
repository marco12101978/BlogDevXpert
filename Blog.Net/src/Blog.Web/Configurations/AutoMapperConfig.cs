using AutoMapper;
using Blog.Business.Models;
using Blog.Web.ViewModels;

namespace Blog.Web.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Autor, AutorViewModel>().ReverseMap();
            CreateMap<Postagem, PostagemViewModel>().ReverseMap();
            CreateMap<Comentario, ComentarioViewModel>().ReverseMap();    
        }


    }
}
