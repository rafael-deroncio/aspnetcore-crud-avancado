using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAvancado.Models
{
    [Table("Controle_Categorias")]
    public class CategoriaModel
    {
        [Key]
        public int IdCategoria { get; set; }
        
        [Required, MaxLength(128)]
        public string Nome { get; set; }

        public DateTime? DataCadastro { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }

        public bool Ativo { get; set; }

        public ICollection<ProdutoModel> Produtos { get; set; }
    }
}