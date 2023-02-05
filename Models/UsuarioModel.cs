using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDAvancado.Models
{
    [Table("Controle_Usuarios")]
    public class UsuarioModel
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(128)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(128)]
        public string Email { get; set; }

        [MaxLength(128)]
        public string Senha { get; set; }

        [ReadOnly(true)]
        public DateTime? DataCadastro { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }


    }
}