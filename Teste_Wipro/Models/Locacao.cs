using System;
using System.ComponentModel.DataAnnotations;

namespace Teste_Wipro.Models
{
    public class Locacao
    {
        [Key]
        public int LocacaoId { get; set; }
        [Required]
        public int FilmeId { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public int DiasLocacao { get; set; }
        public bool Devolvido { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool? Excluido { get; set; }
        public DateTime? DataExcluido { get; set; }
    }
}
