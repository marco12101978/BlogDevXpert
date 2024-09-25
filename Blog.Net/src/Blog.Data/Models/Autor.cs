using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Models
{
    public class Autor
    {
        [Key]
        public Guid IdAutor { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Nome { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(500)]
        public string? Biografia { get; set; }

        public List<Postagem>? Postagens { get; set; }
    }
}
