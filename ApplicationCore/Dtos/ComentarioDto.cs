namespace Application.Dtos
{
    public class ComentarioDto
    {
        public string Descricao { get; set; } = default!;
        /// <summary>
        /// Usuario a efetuar o comentario.
        /// </summary>
        public string Responsavel { get; set; } = default!;
    }
}
