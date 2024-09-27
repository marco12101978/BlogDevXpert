using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog.Web.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
          //  CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
          //  CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
          //  CreateMap<Produto, ProdutoViewModel>().ReverseMap();
        }
    }
}
