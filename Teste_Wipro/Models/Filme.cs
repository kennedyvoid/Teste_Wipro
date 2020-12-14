using System;
using System.ComponentModel.DataAnnotations;

namespace Teste_Wipro.Models
{
    public class Filme
    {
        [Key]
        public int FilmeId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(50)]
        public string Genero { get; set; }
        public bool Alugado { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool? Excluido { get; set; }
        public DateTime? DataExcluido { get; set; }
    }
}
