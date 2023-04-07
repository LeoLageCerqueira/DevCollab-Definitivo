using System.ComponentModel.DataAnnotations;

namespace DevCollab.Domain.Entities
{
	public class Comentario
	{
		[Key]
		public int IdComentario { get; set; }
		public string Texto { get; set; }

		public Guid AutorId { get; set; }

		public int PublicacaoId { get; set; }
		public Publicacao Publicacao { get; set; }
	}
}
