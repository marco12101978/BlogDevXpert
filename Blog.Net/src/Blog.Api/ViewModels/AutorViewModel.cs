using System.ComponentModel.DataAnnotations;

namespace Blog.Api.ViewModels
{
    public class AutorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public string? Nome { get; set; }

        public string? Email { get; set; }

        public string? Biografia { get; set; }

        public List<PostagemViewModel>? Postagens { get; set; }
    }
}
