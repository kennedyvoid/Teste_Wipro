using System;
using System.ComponentModel.DataAnnotations;

namespace Teste_Wipro.Models
{
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool? Excluido { get; set; }
        public DateTime? DataExcluido { get; set; }
    }
}
