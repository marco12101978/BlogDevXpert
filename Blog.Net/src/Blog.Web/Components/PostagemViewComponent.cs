using AutoMapper;
using Blog.Business.Interfaces;
using Blog.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Components
{
    public class PostagemViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly IPostagemRepository _postagemRepository;
        private readonly IAppIdentityUser user;

        protected Guid UserId { get; set; }
        protected string UserName { get; set; }
        protected bool UserAdmin { get; set; }

        public PostagemViewComponent(IMapper mapper,
                                     IPostagemRepository postagemService,
                                     IAppIdentityUser user)
        {
            _mapper = mapper;
            _postagemRepository = postagemService;


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

            return View(_mapper.Map<List<PostagemViewModel>>(await _postagemRepository.ObterTodasPostagem()));
        }



    }
}
