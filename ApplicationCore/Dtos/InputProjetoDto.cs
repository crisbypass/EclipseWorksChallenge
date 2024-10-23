using System.ComponentModel.DataAnnotations;
using Application.Messages;

namespace Application.Dtos
{
    public class InputProjetoDto
    {
        /// <summary>
        /// Responsavel pela criação do projeto.
        /// </summary>
        [Required(ErrorMessage = Mensagens.Requerido)]
        public string NomeUsuario { get; set; } = default!;
    }
}