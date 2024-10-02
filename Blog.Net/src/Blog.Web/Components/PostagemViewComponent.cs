using Blog.Business.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Components
{
    public class PostagemViewComponent : ViewComponent
    {
        private readonly IPostagemRepository postagemRepository;
        private readonly IAppIdentityUser user;

        protected Guid UserId { get; set; }
        protected string UserName { get; set; }
        protected bool UserAdmin { get; set; }

        public PostagemViewComponent(IPostagemRepository postagemService, IAppIdentityUser user)
        {
            postagemRepository = postagemService;
            this.user = user;

            if (user.IsAuthenticated())
            {
                UserId = user.GetUserId();
                UserName = user.GetUsername();
                UserAdmin = user.IsInRole("Admin");
            }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.IdUser = UserId;
            ViewBag.Admin = UserAdmin;

            var postagem = await postagemRepository.ObterTodasPostagem();
            return View(postagem);
        }
    }
}
