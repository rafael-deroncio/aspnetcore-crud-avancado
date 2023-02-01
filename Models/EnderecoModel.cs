using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CRUDAvancado.Models
{
    [Owned]
    [Table("Controle_Enderecos")]
    public class EnderecoModel
    {
        public int IdEndereco { get; set; }
        
        [Required]
        public string Logradouro { get; set; }
        
        [Required]
        public string Numero { get; set; }
        
        [Required]
        public string Complemento { get; set; }
        
        [Required]
        public string Bairro { get; set; }

        [Required]
        public string Cidade { get; set; }
        
        [Required]
        [Column(TypeName = "char(2)")]
        public string Estado { get; set; }
        
        [Required]
        [Column(TypeName = "char(9)")]
        public string CEP { get; set; }
        
        public string Referencia { get; set; }

        public bool Selecionado { get; set; }

        [NotMapped]
        public string EnderecoCompleto { get { return $"{Logradouro}, {Numero} {Complemento}, {Bairro}, {Cidade}-{Estado}, CEP: {CEP}"; } }
    }
}