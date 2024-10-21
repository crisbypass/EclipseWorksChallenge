using System.ComponentModel.DataAnnotations;

namespace EclipseWorksChallenge.MyModels
{
    public class ComentarioModel
    {
        /// <summary>
        /// Valor recuperado a partir da rota.
        /// </summary>
        public int TarefaId { get; set; }
        [Required(ErrorMessage = "O campo {0} é requerido.")]
        public string Comentario { get; set; } = default!;
        /// <summary>
        /// Usuario a efetuar o comentario.
        /// </summary>
        public string Responsavel { get; set; } = default!;
    }
}
