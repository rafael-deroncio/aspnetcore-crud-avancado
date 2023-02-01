using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDAvancado.Models
{
    [Table("Controle_Produtos")]
    public class ProdutoModel
    {
        [Key]
        public int IdProduto { get; set; }

        [Required, MaxLength(128)]
        public string Nome { get; set; }

        public double Valor { get; set; }

        public int Quantidade { get; set; }

        [DefaultValue(0)]
        public DateTime? DataCadastro { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }

        public int IdCategoria { get; set; }

        [ForeignKey("IdCategoria")]
        public CategoriaModel Categoria { get; set; }
    }
}