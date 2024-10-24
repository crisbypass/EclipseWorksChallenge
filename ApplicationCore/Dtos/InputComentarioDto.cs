using System.ComponentModel.DataAnnotations;
using Application.Messages;

namespace Application.Dtos
{
    public class InputComentarioDto
    {
        [Required(ErrorMessage = Mensagens.Requerido)]
        public string Descricao { get; set; } = default!;
        /// <summary>
        /// Usuario a efetuar o comentario.
        /// </summary>
        [Required(ErrorMessage = Mensagens.Requerido)]
        public string NomeUsuario { get; set; } = default!;
    }
}
