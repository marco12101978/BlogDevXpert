using Blog.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Components
{
    public class PostagemViewComponent : ViewComponent
    {
        private readonly IPostagemRepository postagemRepository;

        public PostagemViewComponent(IPostagemRepository postagemService)
        {
            postagemRepository = postagemService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var postagem = await postagemRepository.ObterTodasPostagem();
            return View(postagem);
        }
    }
}
