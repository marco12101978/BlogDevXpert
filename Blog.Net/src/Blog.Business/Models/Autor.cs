namespace Blog.Business.Models
{
    public class Autor : Entity
    {
        public string? Nome { get; set; }

        public string? Email { get; set; }

        public string? Biografia { get; set; }

        public List<Postagem>? Postagens { get; set; }
    }
}
